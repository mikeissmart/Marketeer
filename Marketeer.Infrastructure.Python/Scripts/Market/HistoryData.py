import sys
from datetime import date
import json
import yfinance as yf
import pandas as pd

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

# yfinance wiki
# https://analyzingalpha.com/yfinance-python
if data['StartDate'] == None:
    data['StartDate'] = '1900-01-01'
if data['EndDate'] == None:
    data['EndDate'] = date.today().strftime('%Y-%m-%d')
    
ticker = yf.Ticker(data['Ticker'])
hist_data = ticker.history(start=data['StartDate'], end=data['EndDate'],
                         interval=data['Interval'], period='1d')
if len(hist_data.index) > 0:
    hist_data['DateTime'] = hist_data.index.strftime('%Y-%m-%dT%H:%M:%S%z')
    hist_data = pd.DataFrame(hist_data, columns=['DateTime','Open','Close','High','Low','Volume'])
    out_data = hist_data.to_json(orient='records')
else:
    out_data = '[]'

fp.seek(0)
fp.write(out_data)
fp.truncate()
fp.close()