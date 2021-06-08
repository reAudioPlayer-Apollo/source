function httpGet(theUrl) {
  var xmlHttp = new XMLHttpRequest();
  xmlHttp.open("GET", theUrl, false); // false for synchronous request
  xmlHttp.send(null);
  console.log(xmlHttp.response);

  if (xmlHttp.status == 401) {
    alert(xmlHttp.response);
  }

  return xmlHttp.response;
}

var sdata = httpGet("data/radio");
var jdata = JSON.parse(sdata);
console.log(jdata);

function getAsElement(programme) {
  let t = "<li class='event' data-date='" + programme.time + "'><h3>" + programme.name + "</h3><p>" + programme.description + "</p>";

  for (let i = 0; i < programme.songs.length && i < 4; i++) {
    t += "<p>" + programme.songs[i] + "</p>";
  }

  t += "</li>";

  return t;
}

function getAllAsElement(programmes) {
  let t = "";

  for (let i = 0; i < programmes.length; i++) {
    t += getAsElement(programmes[i])
  }

  return t;
}

document.getElementsByClassName('timeline')[0].innerHTML = getAllAsElement(jdata)