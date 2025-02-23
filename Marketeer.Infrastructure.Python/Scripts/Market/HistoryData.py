﻿import sys
from datetime import date
import json
import yfinance as yf
import pandas as pd

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

# yfinance wiki
# https://analyzingalpha.com/yfinance-python    
ticker = yf.Ticker(data['Ticker'])
try:
    hist_data = ticker.history(start=data['StartDate'], end=data['EndDate'],
        interval=data['Interval'], period='1d', raise_errors=True)
    
    if len(hist_data.index) > 0:
        hist_data['Date'] = hist_data.index.strftime('%Y-%m-%d')
        hist_data = pd.DataFrame(hist_data, columns=['Date','Open','Close','High','Low','Volume'])
        out_data = '{"error":"","output":' + hist_data.to_json(orient='records') + '}'
    else:
        out_data = '{"error":"","output":[]}'
except Exception as e:
    out_data = '{"error":"'+ str(e) +'","output":[]}'    

fp.seek(0)
fp.write(out_data)
fp.truncate()
fp.close()