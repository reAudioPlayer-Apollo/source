window.ws = new function () {
    class Data {
        displayname() {
            webSocket.send(createMessage("data", "displayname"));
        }

        playlists() {
            webSocket.send(createMessage("data", "playlists"));
        }

        playlist(index) {
            webSocket.send(createMessage("data", "playlist", index));
        }

        search(query, scope = "playlist") {
            webSocket.send(createMessage("data", "search", JSON.stringify({
                query,
                scope
            })));
        }

        position() {
            webSocket.send(createMessage("data", "position"));
        }

        volume() {
            webSocket.send(createMessage("data", "volume"));
        }

        cover() {
            webSocket.send(createMessage("data", "cover"));
        }

        radioProgramme() {
            webSocket.send(createMessage("data", "radioProgramme"));
        }

        accentColour() {
            webSocket.send(createMessage("data", "accentColour"));
        }
    }

    class Control {
        next() {
            webSocket.send(createMessage("control", "next"));
        }

        last() {
            webSocket.send(createMessage("control", "last"));
        }

        jump(value) {
            webSocket.send(createMessage("control", "jump", value));
        }

        sort(type) {
            webSocket.send(createMessage("control", "sort", type));
        }

        block(list) {
            webSocket.send(createMessage("control", "block", JSON.stringify(list)));
        }

        playPause() {
            webSocket.send(createMessage("control", "playPause"));
        }

        volume(value) {
            webSocket.send(createMessage("control", "volume", value));
        }

        loadPlaylist(index) {
            webSocket.send(createMessage("control", "load/playlist", index));
        }

        loadAutoPlaylist(name) {
            webSocket.send(createMessage("control", "load/playlist", name));
        }

        load(index, globally = false) {
            globally ? this.loadGlobal(index) : webSocket.send(createMessage("control", "load", index));
        }

        loadGlobal(index) {
            webSocket.send(createMessage("control", "load/global", index));
        }
    }

    class General {
        version(index) {
            webSocket.send(createMessage("general", "version"));
        }
    }

    class Game {
        validateUser(key) {
            webSocket.send(createMessage("game", "validate-user", key));
        }

        library() {
            webSocket.send(createMessage("game", "library"));
        }

        launch(id, user) {
            webSocket.send(createMessage("game", "launch", JSON.stringify({
                id: id,
                user: user
            })));
        }
    }

    class Youtube {
        download(link, output = null) {
            webSocket.send(createMessage("youtube", "download", JSON.stringify({
                link: link,
                output: output
            })));
        }

        sync(link, output = null) {
            webSocket.send(createMessage("youtube", "sync", JSON.stringify({
                link: link,
                output: output
            })));
        }
    }

    class Playlist {
        create(list) {
            webSocket.send(createMessage("playlist", "create", JSON.stringify(list)));
        }

        createVirtual(list) {
            webSocket.send(createMessage("playlist", "virtual/create", JSON.stringify(list)));
        }
    }

    let webSocket = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/ws");
    window.wsConnected = false;

    webSocket.onopen = function (event) {
        console.log("[websocket.js] connected to ws");
        window.wsConnected = true;

        console.log(window.onConnected)

        var evt = document.createEvent("Event");
        evt.initEvent("ws.connect", true, true);

        //invoke
        document.dispatchEvent(evt);
    };

    this.data = new Data();
    this.control = new Control();
    this.general = new General();
    this.game = new Game();
    this.youtube = new Youtube();
    this.playlist = new Playlist();

    this.custom = function (apiModule, endpoint, data = null)
    {
        webSocket.send(createMessage(apiModule, endpoint, data))
    }

    webSocket.onmessage = function (event) {
        try {
        console.log("[websocket.js]", JSON.parse(event.data));

        const jdata = JSON.parse(event.data)

        var evt = document.createEvent("Event");
        const evtName = "ws." + jdata.apiModule + "." + jdata.endpoint;
        console.info(evtName);
        evt.initEvent(evtName, true, true);

        // custom param
        evt.data = jdata;

        //invoke
        document.dispatchEvent(evt);

        }
        catch {
            console.error("could not be parsed")
            console.log("[websocket.js]", event.data)

            if (event.data instanceof Blob)
            {
                event.data.type = "audio/mp3"
                window.blob = event.data

                var evt = document.createEvent("Event");
                const evtName = "ws.streaming.blob";
                console.info(evtName);
                evt.initEvent(evtName, true, true);
                evt.data = event.data;
                document.dispatchEvent(evt);
            }
        }
    }

    function createMessage(apiModule, endpoint, data = null) {
        const st = JSON.stringify({
            apiModule: apiModule,
            endpoint: endpoint,
            data: data
        });

        console.log("[websocket.js] " + st)

        return st;
    }
}