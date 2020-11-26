const puppeteer = require('puppeteer');

(async () => {
  const browser = await puppeteer.launch({headless: false , slowMo: 120});
  const page = await browser.newPage();
  await page.goto("file:///D:/internship/Internship%20Project/Auth/studentTable.html");
  await delay(1000);
  await page.click('#add-new-student');
  await page.type('#username', 'Sadeem');
  await page.type('#email', 'sadeem@gmail.com');
  await page.type('#phone', '03160578748');
  await page.type('#dob', '12/12/2012');
  await page.type('#password', 'Sadeem1');
  await page.type('#confirm-password', 'Sadeem1');
  await page.select('#select-courses','1','2','3');
  await page.click(".saveButton");
  await delay(1000);
  await page.click("#delete-0");
  await delay(1000);
  await page.click("#edit-0");
  await page.type('#dob', '12/12/2012');
  await page.click(".saveButton");
  await browser.close();
})();

function delay(time) {
  return new Promise(function(resolve) { 
      setTimeout(resolve, time)
  });
}