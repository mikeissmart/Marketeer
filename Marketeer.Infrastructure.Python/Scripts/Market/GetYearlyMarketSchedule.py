import sys
import pandas_market_calendars as mcal
import json

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

year = data['Year']

nyse = mcal.get_calendar('NYSE')
data = nyse.schedule(start_date=str(year) + "-01-01", end_date=str(year) + "-12-31")
data['Day'] = data.index.strftime('%Y-%m-%d')
data['MarketOpen'] = data['market_open'].dt.strftime('%Y-%m-%dT%H:%M:%S%z')
data['MarketClose'] = data['market_close'].dt.strftime('%Y-%m-%dT%H:%M:%S%z')
out_data = data.to_json(orient='records')

fp.seek(0)
fp.write(out_data)
fp.truncate()
fp.close()