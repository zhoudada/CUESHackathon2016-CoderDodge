from app import app
#from app import callPredict
from flask import render_template
from flask import request
#import models
import os
import json
from flask import jsonify
#from datetime import datetime as dt


@app.route("/")
def index():
        return render_template("index.html")

score_list = [0, 0, 0]

@app.route("/highestScores",methods = ['GET'])
def get_data():
    #end = request.args.get('to')
    #print data
    '''
    SITE_ROOT = os.path.realpath(os.path.dirname(__file__))
    json_url = os.path.join(SITE_ROOT, "static/data","predict.json")
    with open(json_url) as json_file:
        data = json.load(json_file)
    '''
    data = {'first': score_list[0],'second': score_list[1],'third':score_list[2]}
    return jsonify(data)


@app.route("/updateScores",methods = ['POST'])
def update_data():
    score = int(request.form['newScore'])
    #score = request.args.get('newScore')
    global score_list
    score_list.append(score)
    score_list.sort(reverse=True)
    score_list = score_list[:3]
    
    print('Previous scores: {}, {}, {}, new score: {}'.format(score_list[0], score_list[1], score_list[2], score))
    data = {'first': score_list[0],'second': score_list[1],'third':score_list[2]}
    return jsonify(data)

    #end = request.args.get('to')
    #print data
    '''
    SITE_ROOT = os.path.realpath(os.path.dirname(__file__))
    json_url = os.path.join(SITE_ROOT, "static/data","predict.json")
    with open(json_url) as json_file:
        data = json.load(json_file)
    '''

'''
@app.route("/:month",methods=['GET'])
def pass_predict():
        predict = models.get_local_json()
        return render_template('index.html',s_data = predict);
'''
