from flask import Flask,render_template,request
app = Flask(__name__)
@app.route('/RA/api/v1.0/Welcome', methods=['POST'])
def Welcome():
    Name = ""
    try:
        Name = request.form['Name']
        return (" Welcome to DotNet World " + Name)
    except:
        return ("Some Error Occured")
if __name__ == '__main__':
    app.run(host='localhost',port='5000',debug=True)

