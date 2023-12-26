import sys
import pandas as pd
import json
import numpy as np
from transformers import AutoTokenizer
from transformers import AutoModelForSequenceClassification
from tqdm.notebook import tqdm
from transformers import AutoTokenizer
from transformers import AutoModelForSequenceClassification
import torch

dataFile = sys.argv[1]
fp = open(dataFile, 'r+', encoding='utf-8')
data = json.loads(fp.read())

queue = data['Queue']

MODEL = data['HuggingFaceModel']
tokenizer = AutoTokenizer.from_pretrained(MODEL)
model = AutoModelForSequenceClassification.from_pretrained(MODEL)

def func(item):
    # normal tokenizer usage
    encoded_text = tokenizer(item['Text'],
                            return_tensors='pt',
                            add_special_tokens=False,
                            max_length=512,
                            padding='max_length',
                            truncation=False
                            )

    # break into chunks    
    input_id_chunks = encoded_text['input_ids'][0].split(510)
    input_mask_chunks = encoded_text['attention_mask'][0].split(510)
    input_id_chunks = list(input_id_chunks)
    input_mask_chunks = list(input_mask_chunks)
    
    for i in range(len(input_id_chunks)):
        input_id_chunks[i] = torch.cat([
            torch.Tensor([101]), input_id_chunks[i], torch.Tensor([102])
        ])
        input_mask_chunks[i] = torch.cat([
            torch.Tensor([1]), input_mask_chunks[i], torch.Tensor([1])
        ])
        pad_len = 512 - input_id_chunks[i].shape[0]
        if pad_len > 0:
            input_id_chunks[i] = torch.cat([
                input_id_chunks[i], torch.Tensor([0] * pad_len)
            ])
            input_mask_chunks[i] = torch.cat([
                input_mask_chunks[i], torch.Tensor([0] * pad_len)
            ])
    
    input_ids = torch.stack(input_id_chunks)
    input_mask = torch.stack(input_mask_chunks)
    
    encoded_text = {
        'input_ids': input_ids.long(),
        'attention_mask': input_mask.int()
    }
    
    output = model(**encoded_text)
    scores = torch.nn.functional.softmax(output[0], dim=-1)
    scores = scores.mean(dim=0)
    
    scores_dict = {
        'ItemGuid': item['ItemGuid'],
        'Negative': scores[0].item(),
        'Neutral': scores[1].item(),
        'Positive': scores[2].item()
    }
    return scores_dict

out_data = []
for q in queue:
    out_data.append(func(q))

fp.seek(0)
fp.write(json.dumps({
    'Queue': out_data
}))
fp.truncate()
fp.close()