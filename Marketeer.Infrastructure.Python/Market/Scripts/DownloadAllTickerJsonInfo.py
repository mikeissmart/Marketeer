import yfinance as yf
import pyodbc as db
import pandas as pd
import json

dbContext = db.connect("Driver={SQL Server};Server=.;Database=Marketeer;Trusted_Connection=True;MultipleActiveResultSets=true")
dbCursor = dbContext.cursor()

dbCursor.execute("SELECT Symbol FROM [Marketeer].[dbo].[Tickers] order by Symbol")
allSymbols = dbCursor.fetchall()

doneSymbols = dbCursor.execute("SELECT Symbol FROM [Marketeer].[dbo].[JsonTickerInfos] order by Symbol")

for d in doneSymbols:
    allSymbols.remove(d)

countPerBatch = 100
batchCount = int(len(allSymbols) / countPerBatch)
if len(allSymbols) % countPerBatch != 0:
    batchCount = batchCount + 1

for b in range(batchCount):
    batchSymbols = ''
    for i in range(countPerBatch):
        batchSymbols = batchSymbols + allSymbols[(b * countPerBatch) + i][0] + ' '
    batchSymbols = batchSymbols.strip()

    tickers = yf.Tickers(batchSymbols)
    symbols = batchSymbols.split(' ')

    for i in range(countPerBatch):
        tickerInfo = tickers.tickers[symbols[i]].info
        jsonData = json.dumps(tickerInfo)
        jsonData = jsonData.replace("'", "''")
        sql = "INSERT INTO [Marketeer].[dbo].[JsonTickerInfos] ([Symbol], [InfoJson]) values ('" + symbols[i] + "', '" + jsonData + "')"
        dbCursor.execute(sql)
        dbCursor.commit()
        print(f'{i} - {symbols[i]}')