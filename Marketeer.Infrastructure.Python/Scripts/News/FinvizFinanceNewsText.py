import sys
import json
from newsplease import NewsPlease

dataFile = sys.argv[1]
fp = open(dataFile, 'r+')
data = json.loads(fp.read())

links = data['Links']    
result = []
for row in links:
    article = NewsPlease.from_url(row)
    if article.maintext is not None and len(article.maintext) > 0:
        result.append({
            'link': row,
            'text': article.maintext})
        
fp.seek(0)
fp.write(json.dumps(result))
fp.truncate()
fp.close()