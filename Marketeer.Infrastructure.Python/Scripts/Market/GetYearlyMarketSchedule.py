import sys
import pandas_market_calendars as mcal
import json

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

year = data['Year']

nyse = mcal.get_calendar('NYSE')
data = nyse.schedule(start_date=str(year) + "-01-01", end_date=str(year) + "-12-31")
data['Date'] = data.index.strftime('%Y-%m-%d')
out_data = data.to_json(orient='records')

fp.seek(0)
fp.write(out_data)
fp.truncate()
fp.close()