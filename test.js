var webPage = require('webpage');
var page = webPage.create();


page.paperSize = {
    format:'A4'
}
page.viewportSize = {
    width: 1280,
    height: 800
}


page.setContent("<html><body><div>Hello World!</div></body></html>","http://www.baidu.com");
page.render("out.png");
console.log("Done rendering.");
phantom.exit();


/*
page.open('http://www.baidu.com/', function(status) {
  console.log('Status: ' + status);
  // Do other things here...
  page.render("out.jpg");
  console.log("Done rendering.");
  phantom.exit();
});
*/
