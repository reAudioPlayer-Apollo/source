const wsurl = "eu-apollo.herokuapp.com"; // eu-apollo.herokuapp.com

const db = [{
  "name": "Assassin's Creed Valhalla",
  "cover": "//images.igdb.com/igdb/image/upload/t_1080p/co2ed3.jpg",
  "igdbId": 133004,
  "exe": ["ACValhalla.exe", "ACValhalla_Plus.exe"]
}, {
  "name": "Assassin's Creed: Odyssey",
  "cover": "//images.igdb.com/igdb/image/upload/t_1080p/co2nul.jpg",
  "igdbId": 103054,
  "exe": ["ACOdyssey.exe"]
}, {
  "name": "Assassin's Creed",
  "cover": "//images.igdb.com/igdb/image/upload/t_1080p/co2gjx.jpg",
  "igdbId": 1970,
  "exe": ["AssassinsCreed_Dx9.exe", "AssassinsCreed_Dx10.exe", "AssassinsCreed_Game.exe"]
}];

/***********************************************/
/* SOCKET */

var socket = io(wsurl);
window.connected = false;

socket.on('authorised', function (data) {
  console.log("authorised")
  document.getElementById("shareLibraryCode").innerHTML = "Share your Library: " + localStorage.getItem('client')
    .toUpperCase();
  window.connected = true;
  socket.emit('synchronise game database', db);
});

// receive game invite
socket.on('game invite', function (data) {
  if (document.getElementById("shareIn").value == data.inviter.toUpperCase()) {
    //const json = JSON.parse(data.message);
    const json = data;

    if (json.receiver == localStorage.getItem('client').toUpperCase()) {
      const game = window.library.find(it => it.igdbId == json.gameId);
      launchGame(game.name, game.igdbId, "Your friend wants you to play $NAME!", false);
    }
  }
});

// receive friends library
socket.on('game library of', function (data) {
  const friendLib = data;
  const library = document.getElementById("library");

  for (let i = 0; i < library.children.length; i++) {
    const game = library.children[i];
    const name = game.getElementsByTagName('p')[0].innerHTML;

    if (isEligible(game, name, friendLib)) {
      if (game.classList.contains("hidden")) {
        game.classList.remove("hidden");
      }
    } else {
      if (!game.classList.contains("hidden")) {
        game.classList.add("hidden");
      }
    }
  }
})

// invite to game
function launchFriend(id) {
  socket.emit('launch game', id);
}

// authorisation; also publishs game library
function shareLibrary() {
  const key = localStorage.getItem('client');

  const body = {
    "value": JSON.stringify(window.library),
    "key": key.toUpperCase()
  }

  console.log(body);

  socket.emit('authorise', body);

  console.log("Content-Length:", JSON.stringify(body).length)
}

// updates lib while concidering friend's lib
function updateLib() {
  const shareIn = document.getElementById("shareIn");
  const v = shareIn.value
  socket.emit('get game library of', v);
}

/***********************************************/
/* FUNS */

document.addEventListener("ws.game.validate-user", (evt) => localStorage.setItem('client', evt.data.data));
document.addEventListener("ws.connect", onConnected);
document.addEventListener("ws.game.library", receiveLibrary);
document.addEventListener("ws.game.launch", receiveLaunch);

function onConnected() {
  ws.game.validateUser(localStorage.getItem("client") || "1");
  ws.game.library();
}

function receiveLibrary(evt) {
  const jdata = JSON.parse(evt.data.data);
  window.library = jdata;

  let library = "";

  for (let i = 0; i < jdata.length; i++) {
    var line = "<div class='game'><img onclick='launchGame(\"" + jdata[i].name.replace("'", "") + "\"," + jdata[i]
      .igdbId + ")' src='" +
      jdata[i].cover + "'><p>" + jdata[i].name + "</p></div>";

    library += line;

  }
  document.getElementById("library").innerHTML = library;
  shareLibrary();
}

function receiveLaunch(evt) {
  if (evt.data.data != "OK") {
    alert(evt.data.data);
  }
}

function launchGame(name, id, confirmMsg = "Do you want to launch $NAME?", askFriend = true) {
  name = name.replace("'", "");

  if (confirm(confirmMsg.replace("$NAME", name))) {
    ws.game.launch(id, localStorage.getItem("client"));

    if (askFriend) {
      const shareIn = document.getElementById("shareIn").value;

      const req = {
        id: id,
        receiver: shareIn
      };
      console.log(req)
      launchFriend(req);
    }
  }
}

function launchRandom() {
  const game = jdata[Math.floor(Math.random() * (jdata.length - 1))];
  launchGame(game.name, game.igdbId);
}

function launchFavourite() {

}

function launchRecent() {

}

function isEligible(game, name, friendLib) {
  const search = document.getElementById("search");

  return (name.toLowerCase().includes(search.value.toLowerCase()) || search.value == "") &&
    (friendLib == null || friendLib == "null" || !!friendLib.find(game => game.name == name))
}