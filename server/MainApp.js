const express = require("express");
const cors = require("cors");
require("dotenv").config();
const db = require("./config/database");
const { parse } = require("url");
const bcrypt = require("bcrypt");
const app = express();
const firebaseapp = require("./config/firebase");

const port = process.env.PORT || 5000;
const server = require("http").createServer(app);
const WebSocket = require("ws");

const radareWss = new WebSocket.Server({ noServer: true });
const droneWss = new WebSocket.Server({ noServer: true });
const appWss = new WebSocket.Server({ noServer: true });

const Event = require("./models/Event");
const User = require("./models/User");

app.use(cors());
app.use(express.json());

//start server
const startServer = () => {
  server.on("upgrade", function upgrade(request, socket, head) {
    const { pathname } = parse(request.url);

    if (pathname === "/radar") {
      radareWss.handleUpgrade(request, socket, head, function done(ws) {
        radareWss.emit("connection", ws, request);
      });
    } else if (pathname === "/drone") {
      droneWss.handleUpgrade(request, socket, head, function done(ws) {
        droneWss.emit("connection", ws, request);
      });
    } else if (pathname === "/app") {
      appWss.handleUpgrade(request, socket, head, function done(ws) {
        appWss.emit("connection", ws, request);
      });
    } else {
      socket.destroy();
    }
  });

  radareWss.on("connection", function connection(ws) {
    ws.on("message", async (msg) => {
      const client_msg = JSON.parse(msg);
      const event = await saveEvents(client_msg.data);
      sendEventToApp(event);
    });
  });

  droneWss.on("connection", function connection(ws) {
    ws.on("message", async (msg, isBinary) => {
      if (!isBinary) {
        appWss.clients.forEach(function each(client) {
          if (client.readyState === WebSocket.OPEN) {
            client.send(msg, { binary: false });
          }
        });
      }
    });
  });

  appWss.on("connection", function connection(ws) {
    ws.on("message", async (msg, isBinary) => {
      droneWss.clients.forEach(function each(client) {
        if (client.readyState === WebSocket.OPEN) {
          client.send(msg + "", { binary: false });
        }
      });
    });
  });

  db.authenticate()
    .then(() => {
      console.log("Database connected...");
    })
    .catch((err) => console.log("Error: " + err));

  server.listen(port, () =>
    console.log(`Server started on port ${port}`)
  );
};

// send Coordinates to drone
const sendEventToApp = (data) => {
  appWss.clients.forEach(function each(client) {
    if (client.readyState === WebSocket.OPEN) {
      client.send(JSON.stringify(data), { binary: false });
    }
  });
};

/// save events
const saveEvents = async (event) => {
  let createdEvent;
  try {
    createdEvent = await Event.create(event);
  } catch (err) {}

  return createdEvent;
};

// update event  => http put request
app.put("/api/events/:id", async function getEvents(req, res) {
  const id = req.params.id;
  const reqData = req.body;
  let updated;
  try {
    updated = await Event.update(reqData, { where: { id: id } });
  } catch (err) {
    return res.status(500).json({ error_message: err.message });
  }
  if (!updated)
    return res.status(404).json({ error_message: "Events not updated!" });

  res.status(200).json({ events: updated });
});

//get events => http get request
app.get("/api/events/", async function getEvents(req, res) {
  let foundEvents;
  try {
    foundEvents = await Event.findAll();
  } catch (err) {
    return res.status(500).json({ error_message: err.message });
  }
  if (!foundEvents)
    return res.status(404).json({ error_message: "Events not found!" });

  res.status(200).json({ events: foundEvents });
});

//get  events by status => http get request
app.get("/api/events/", async function getEventsBystatus(req, res) {
  let foundEvents;
  const qstatus = req.query.status;
  try {
    if (qstatus)
      foundEvents = await Event.findAll({ where: { status: qstatus } });
    else foundEvents = await Event.findAll();
  } catch (err) {
    return res.status(500).json({ error_message: err.message });
  }
  if (!foundEvents)
    return res.status(404).json({ error_message: "Events not found!" });

  res.status(200).json({ events: foundEvents });
});

//login => http  post request
app.post("/api/auth/login", async function login(req, res) {
  const userData = req.body;
  let foundUser;
  //check if user exist
  try {
    foundUser = await User.findOne({ where: { username: userData.username } });
  } catch (err) {
    return res.status(500).json({ error_message: err.message });
  }

  if (!foundUser)
    return res.status(404).json({ error_message: "user not found!" });

  let validPassword;
  try {
    validPassword = await bcrypt.compare(userData.password, foundUser.password);
  } catch (err) {
    console.log(err);
    return res.status(500).json({ error_message: err.message });
  }

  if (!validPassword)
    return res
      .status(404)
      .json({ error_message: "Wrong user name, password!" });

  res.status(200).json(foundUser);
});

const map = () => {};

//send message
const SendMassage = () => {};

//save report
const SaveReports = (report) => {};

//save image to cloud
const SaveImages = (img) => {};

startServer();
