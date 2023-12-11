import sys
import json
from finvizfinance.news import News
from finvizfinance.quote import finvizfinance
from newsplease import NewsPlease

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

symbol = data['Symbol']
if len(symbol) > 0:
    stock = finvizfinance(symbol)
    news = stock.ticker_news()
else:
    news = News().get_news()['news']
    
result = []
for i, row in news.iterrows():
    article = NewsPlease.from_url(row['Link'])
    if article.maintext is not None and len(article.maintext) > 0:
        result.append({
            'title': row['Title'],
            'link': row['Link'],
            'date': row['Date'],
            'text': article.maintext})
        
fp.seek(0)
fp.write(json.dumps(result))
fp.truncate()
fp.close()