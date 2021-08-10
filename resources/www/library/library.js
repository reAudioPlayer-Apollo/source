document.addEventListener("ws.connect", onConnected);
document.addEventListener("ws.data.search", receiveLibrary);
document.addEventListener("ws.control.sort", updateLib);
document.addEventListener("ws.data.displayname", filterNowPlaying);
document.addEventListener("ws.data.playlists", populatePlaylists);
document.addEventListener("ws.control.load/playlist", updateLib);

function onConnected() {
  console.time("resp")
  ws.data.playlists();
}

function changeView(index)
{
  const library = document.getElementById("library");

  for (let i = 0; i < library.children.length; i++) {
    const game = library.children[i];

    if (index == 0)
    {
      if (game.classList.contains("compact")) {
        game.classList.remove("compact");
      }
    } else {
      if (!game.classList.contains("compact")) {
        game.classList.add("compact");
      }
    }
  }
}

function populatePlaylists(evt)
{
  const select = document.getElementById("playlist");
  const playlists = JSON.parse(evt.data.data)

  const si = select.children[select.selectedIndex].outerText;
  while(select.children.length > 1)
  {
    select.removeChild(select.children[1]);
  }

  autoPlaylistsCount = playlists.autoplaylists.length;
  for (let i = 0; i < playlists.autoplaylists.length; i ++)
  {
    option = document.createElement('option');
    option.setAttribute('value', "★ " + playlists.autoplaylists[i].name);
    option.appendChild(document.createTextNode("★ " + playlists.autoplaylists[i].name));
    select.appendChild(option);
  }

  for (let i = 0; i < playlists.customplaylists.length; i ++)
  {
    option = document.createElement('option');
    option.setAttribute('value', playlists.customplaylists[i].name);
    option.appendChild(document.createTextNode(playlists.customplaylists[i].name));
    select.appendChild(option);
  }

  select.selectedIndex = Array.prototype.slice.call(select.children).findIndex(x => x.outerText == si);

  updateLib();
}

function updateLib()
{
  ws.data.search(document.getElementById("search").value, document.getElementById("scope").value);
}

function updateBlock()
{
  const library = document.getElementById("library");
  const children = Array.prototype.slice.call( library.children );
  const blockedElements = children.filter(x => x.classList.contains("hidden"));
  console.log(blockedElements);
  const blockList = blockedElements.map(x => children.findIndex(y => y == x));
  console.log(blockList);
  ws.control.block(blockList);
}

function createPlaylist() {
  const library = document.getElementById("library");
  const children = Array.prototype.slice.call( library.children );
  const blockedElements = children.filter(x => !x.classList.contains("hidden"));
  const blockList = blockedElements.map(x => children.findIndex(y => y == x));

  confirm(`Do you want to create a playlist with this ${blockList.length} songs?`) ? ws.control.createPlaylist(blockList) : null;
}

function debounce(func, timeout = 300){
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => { func.apply(this, args); }, timeout);
  };
}

function filterNowPlaying(evt)
{
  const displayname = evt.data.data;
  const library = document.getElementById("library");

  for (let i = 0; i < library.children.length; i++) {
    const game = library.children[i];

    if (jdata[i].oneLiner != displayname)
    {
      if (game.classList.contains("focus")) {
        game.classList.remove("focus");
      }
    } else {
      if (!game.classList.contains("focus")) {
        game.classList.add("focus");
      }
    }
  }
}

function filterLib()
{
  let search = document.getElementById("search").value;
  const library = document.getElementById("library");

  const invertedSearch = search.substr(0, 1) == "-";
  search = search.substr(0, 1) == "-" ? search.substr(1) : search;

  const onmatch = invertedSearch ? hide : show;
  const onnomatch = invertedSearch ? show : hide;

  if (search.length == 0)
  {
    for (let i = 0; i < library.children.length; i++) { show(library.children[i].classList) }
    return;
  }

  for (let i = 0; i < library.children.length; i++) {
    const game = library.children[i];

    const results = search.split(' ').filter(x => jdata[i].keywords.toLowerCase().includes(x.toLowerCase()));

    if (results && results.length == search.split(' ').length)
    {
      onmatch(game.classList);
    } else {
      onnomatch(game.classList);
    }
  }
}

function show(classlist)
{
  if (classlist.contains("hidden")) {
    classlist.remove("hidden");
  }
}

function hide(classlist)
{
  if (!classlist.contains("hidden")) {
    classlist.add("hidden");
  }
}

let jdata = { }
let autoPlaylistsCount = 0

function loadPlaylist(index, text)
{
  if (index <= 0)
  {
    return;
  }

  if (index <= autoPlaylistsCount)
  {
    ws.control.loadAutoPlaylist(document.getElementById("playlist").value.substr(2));
  }
  else
  {
    ws.control.loadPlaylist(index - (1 + autoPlaylistsCount));
    ws.data.playlists();
  }
}

function receiveLibrary(evt) {
  console.timeEnd("resp")
  console.time("parse")
  //document.getElementById("loading").classList.add("invisible");
  jdata = JSON.parse(evt.data.data);
  window.library = jdata;
  console.timeEnd("parse")
  console.time("calc")

  let library = "";

  const classes = document.getElementById("viewselect").selectedIndex == 1 ? "game compact" : "game";

  for (let i = 0; i < jdata.length; i++) {
    jdata[i].info.loudness = Math.round(jdata[i].info.loudness / 100);
    var line = "<div class='" + classes +"'><img loading='lazy' style='box-shadow:0px 0px 20px rgba(" + jdata[i].accentColour + ", .6)' onclick='ws.control.load(" + jdata[i].index + ", " + (document.getElementById("scope").value == "Global") + ");' src='" +
      jdata[i].coverUri?.replaceAll('"', '') + "'>"
      + "<h4>" + jdata[i].title + "</h4>"
      + "<h5>" + jdata[i].secondLiner + "</h5>"
      + "<details><summary>Stats</summary><table>"
      + `<tr><td>Play Count<td>${jdata[i].playCount}</td></tr>`
      + `<tr><td>Popularity<td>${jdata[i].info.popularity}</td></tr>`
      + `<tr><td>Energy<td>${jdata[i].info.energy}</td></tr>`
      + `<tr><td>Danceability<td>${jdata[i].info.danceability}</td></tr>`
      + `<tr><td>Happiness<td>${jdata[i].info.happiness}</td></tr>`
      + `<tr><td>Loudness<td>${jdata[i].info.loudness} dB</td></tr>`
      + `<tr><td>Accousticness<td>${jdata[i].info.accousticness}</td></tr>`
      + `<tr><td>Instrumentalness<td>${jdata[i].info.instrumentalness}</td></tr>`
      + `<tr><td>Liveness<td>${jdata[i].info.liveness}</td></tr>`
      + `<tr><td>Speechiness<td>${jdata[i].info.speechiness}</td></tr>`
      + `<tr><td>Key<td>${jdata[i].info.key}</td></tr>`
      + "</table>Released on " + jdata[i].info.releaseDate + "</div>";

      //jdata[i].keywords = `${jdata[i].title} ${jdata[i].artist} ${jdata[i].album} ${getInfoKeywords(jdata[i].info)}` + (document.getElementById("scope").value == "Global" ? ` ${jdata[i].location}` : "");

    library += line;

  }
  document.getElementById("library").innerHTML = library;
  console.timeEnd("calc")
}

const months = [
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December'
]

const days = [
  'Sun',
  'Mon',
  'Tue',
  'Wed',
  'Thu',
  'Fri',
  'Sat'
]

function getInfoKeywords(info)
{
  const d = info.releaseDate ? new Date(info.releaseDate) : new Date();
  return `pop:${info.popularity} nrg:${info.energy} dnc:${info.danceability} `
  + `hap:${info.happiness} loud:${info.loudness} acc:${info.accousticness} inst:${info.instrumentalness} live:${info.liveness} `
  + `spe:${info.speechiness} key:${info.key} dat:${info.releaseDate}` + (info.releaseDate ? ` mon:${months[d.getMonth()]} day:${days[d.getDay()]}` : "");
}