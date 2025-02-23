﻿using FluentFTP;
using Microsoft.Extensions.Logging;
using System;

namespace Marketeer.Infrastructure.External.Market
{
    public interface INasdaqApiClient
    {
        Task<List<string>> AllSymbolsAsync();
    }

    public class NasdaqApiClient : INasdaqApiClient
    {
        protected readonly ILogger _logger;
        protected readonly string _baseUri;

        public NasdaqApiClient(ILogger<NasdaqApiClient> logger)
        {
            _logger = logger;
        }

        /*
        Original
        public async Task<List<string>> AllSymbolsAsync()
        {

            using (var client = new AsyncFtpClient(@"ftp://ftp.nasdaqtrader.com"))
            {
                var file = "";
                try
                {
                    var totalData = new List<string>();
                    await client.AutoConnect();

                    file = "nasdaqlisted.txt";
                    await client.DownloadFile(Path.Combine(Directory.GetCurrentDirectory(), file), @$"SymbolDirectory/{file}",
                         FtpLocalExists.Overwrite, FtpVerify.Delete);
                    using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), file), FileMode.Open))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            totalData = ParseNasdaqFile(reader, totalData);
                        }
                    }
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), file));

                    file = "otherlisted.txt";
                    await client.DownloadFile(Path.Combine(Directory.GetCurrentDirectory(), file), @$"SymbolDirectory/{file}",
                         FtpLocalExists.Overwrite, FtpVerify.Delete);
                    using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), file), FileMode.Open))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            totalData = ParseNasdaqFile(reader, totalData);
                        }
                    }
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), file));

                    await client.Disconnect();

                    totalData = totalData
                        .Where(x => x.Length > 0 && x.Length <= 5)
                        .Distinct()
                        .ToList();

                    return totalData;
                }
                catch (Exception ex)
                {
                    await client.Disconnect();
                    _logger.LogError(ex, $"FTP: File: {file}: Message: {ex.Message}");
                    throw;
                }
            }
        }*/

        public async Task<List<string>> AllSymbolsAsync()
        {

            using (var client = new AsyncFtpClient(@"ftp://ftp.nasdaqtrader.com"))
            {
                var file = "";
                try
                {
                    var totalData = new List<string>();
                    await client.AutoConnect();

                    file = "nasdaqlisted.txt";
                    totalData = await LoadNasdaqFile(file, totalData, client);

                    file = "otherlisted.txt";
                    totalData = await LoadNasdaqFile(file, totalData, client);

                    await client.Disconnect();

                    totalData = totalData
                        .Where(x => x.Length > 0 && x.Length <= 5)
                        .Distinct()
                        .ToList();

                    return totalData;
                }
                catch (Exception ex)
                {
                    await client.Disconnect();
                    _logger.LogError(ex, $"FTP: File: {file}: Message: {ex.Message}");
                    throw;
                }
            }
        }

        private async Task<List<string>> LoadNasdaqFile(string file, List<string> totalData, AsyncFtpClient client)
        {
            var tempFile = Path.GetTempFileName();
            await client.DownloadFile(tempFile, @$"SymbolDirectory/{file}",
                 FtpLocalExists.Overwrite, FtpVerify.Delete);

            using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), file), FileMode.Open,
                FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    totalData = ParseNasdaqFile(reader, totalData);
                }
            }

            return totalData;
        }

        private List<string> ParseNasdaqFile(StreamReader reader, List<string> totalData)
        {
            var header = reader.ReadLine()!.Split('|');
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine()!.Split('|');
                if (line[0].ToLower().Contains("file creation time"))
                    continue;

                var symbol = "";
                for (var i = 0; i < header.Length; i++)
                {
                    switch (header[i])
                    {
                        case "NASDAQ Symbol":
                        case "Symbol":
                            if (line[i].Length >= 6)
                                continue;
                            symbol = line[i];
                            break;
                        case "Test Issue":
                            if (line[i].ToUpper() == "Y")
                                continue;
                            break;
                    }
                }
                totalData.Add(symbol);
            }

            return totalData;
        }
    }
}
