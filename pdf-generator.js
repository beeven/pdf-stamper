"use strict";

var phantom = require("phantom"),
    temp = require("temp"),
    Promise = require("bluebird");


/*
pageSize :
{
  width: '200px',
  height: '300px',
  margin: '0px'
}

{
  format: 'A4',
  orientation: 'portrait',
  margin: '1cm'
}

{
  width: '5in',
  height: '7in',
  margin: {
    top: '50px',
    left: '20px'
  }
}
*/
var defaultPageSize = {
    format: 'A4',
    orientation: 'landscape',
    margin: {
        top: '1cm',
        bottom: '3cm',
        left: '1cm',
        right: '1cm'
    }
}

class PDFGenerator {
    constructor(){
        this.phInstance = null;
        this.sitepage = null;
    }
    init(){
        var that = this;
        if(this.phInstance == null) {
            return phantom.create()
                .then(instance => {
                    that.phInstance = instance;
                    process.on('exit',()=>{
                        that.phInstance.exit();
                    });
                    return instance.createPage();
                })
                .then(page=>{
                    that.sitePage = page;
                    //return page.property('viewportSize',{width: 1280, height: 800});
                });
        } else {
            return Promise.resolve(this.phInstance);
        }
    }

    generatePDF(content, pageSize) {
        var that = this;
        pageSize = pageSize || defaultPageSize;
        return Promise.coroutine(function*(){
            yield that.sitePage.property('pageSize',pageSize);
            var tempName = temp.path({prefix:'pdfgen',suffix:'.pdf'});
            that.sitePage.setContent(content,"http://www.example.com");
            that.sitePage.render(tempName);
            //return Promise.resolve(tempfile);
            return Promise.resolve(tempName);
        })();
    }
}

module.exports = new PDFGenerator();
