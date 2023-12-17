import sys
import json
from finvizfinance.news import News
from finvizfinance.quote import finvizfinance
from newsplease import NewsPlease

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

symbol = data['Symbol']
    
stock = finvizfinance(symbol)
news = stock.ticker_news()

result = []
for i, row in news.iterrows():
    result.append({
        'title': row['Title'],
        'link': row['Link'],
        'date': row['Date'].date().strftime('%Y-%m-%d')})
    
        
fp.seek(0)
fp.write(json.dumps(result))
fp.truncate()
fp.close()