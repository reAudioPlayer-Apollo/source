/***********************************************/
/* FUNS */

document.addEventListener("ws.connect", onConnected);
document.addEventListener("ws.data.search", receiveLibrary)

function onConnected() {
  console.time("resp")
  ws.data.search("");
}

function updateLib()
{
  ws.data.search(document.getElementById("search").value);
}

function debounce(func, timeout = 300){
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => { func.apply(this, args); }, timeout);
  };
}
const processChange = debounce(() => updateLib());

function receiveLibrary(evt) {
  console.timeEnd("resp")
  console.time("parse")
  document.getElementById("loading").classList.add("invisible");
  const jdata = JSON.parse(evt.data.data);
  window.library = jdata;
  console.timeEnd("parse")
  console.time("calc")

  let library = "";

  for (let i = 0; i < jdata.length; i++) {
    var line = "<div class='game'><img <img style='box-shadow:0px 0px 20px rgba(" + jdata[i].accentColour + ", .6)' onclick='ws.control.load(" + jdata[i].index + ");' src='" +
      jdata[i].coverUri?.replaceAll('"', '') + "'>"
      + "<h4>" + jdata[i].title + "</h4>"
      + "<h5>" + jdata[i].secondLiner + "</h5>"
      + "<details><summary>Stats</summary><table>"
      + `<tr><td>Popularity<td>${jdata[i].info.popularity}</td></tr>`
      + `<tr><td>Energy<td>${jdata[i].info.energy}</td></tr>`
      + `<tr><td>Danceability<td>${jdata[i].info.danceability}</td></tr>`
      + `<tr><td>Happiness<td>${jdata[i].info.happiness}</td></tr>`
      + `<tr><td>Loudness<td>${Math.round(jdata[i].info.loudness / 100)} dB</td></tr>`
      + `<tr><td>Accousticness<td>${jdata[i].info.accousticness}</td></tr>`
      + `<tr><td>Instrumentalness<td>${jdata[i].info.instrumentalness}</td></tr>`
      + `<tr><td>Liveness<td>${jdata[i].info.liveness}</td></tr>`
      + `<tr><td>Speechiness<td>${jdata[i].info.speechiness}</td></tr>`
      + `<tr><td>Key<td>${jdata[i].info.key}</td></tr>`
      + "</table>" + jdata[i].info.releaseDate + "</div>";

    library += line;

  }
  document.getElementById("library").innerHTML = library;
  console.timeEnd("calc")
}
