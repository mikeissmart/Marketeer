import yfinance as yf
import pyodbc as db
import pandas as pd
import json

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

dbContext = db.connect("Driver={SQL Server};Server=.;Database=Marketeer;Trusted_Connection=True;MultipleActiveResultSets=true")
dbCursor = dbContext.cursor()

symbols = data['Tickers']

countPerBatch = 100
batchCount = int(len(allSymbols) / countPerBatch)
if len(allSymbols) % countPerBatch != 0:
    batchCount = batchCount + 1

for b in range(batchCount):
    batchSymbols = ''
    for i in range(countPerBatch):
        batchSymbols = batchSymbols + symbols[(b * countPerBatch) + i][0] + ' '
    batchSymbols = batchSymbols.strip()

    tickers = yf.Tickers(batchSymbols)
    symbols = batchSymbols.split(' ')

    for i in range(countPerBatch):
        tickerInfo = tickers.tickers[symbols[i]].info
        jsonData = json.dumps(tickerInfo)
        jsonData = jsonData.replace("'", "''")

        dbCursor.execute("SELECT COUNT(*) FROM [Marketeer].[dbo].[JsonTickerInfos] WHERE Symbol = '" + symbols[i] + '"")
        if dbCursor.fetchall()[0] > 1:
            sql = "UPDATE [Marketeer].[dbo].[JsonTickerInfos] [InfoJson] = '" + jsonData + "' WHERE [Symbol] = '" + symbols[i] + "'"
        else:
            sql = "INSERT INTO [Marketeer].[dbo].[JsonTickerInfos] ([Symbol], [InfoJson]) values ('" + symbols[i] + "', '" + jsonData + "')"
        dbCursor.execute(sql)
        print(f'{i} - {symbols[i]}')
    dbCursor.commit()