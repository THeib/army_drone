const express = require("express");
const cors = require("cors");
require("dotenv").config();


const app = express();
const port = process.env.PORT || 5000;

const authRoute = require("./routes/auth");
const eventRoute = require("./routes/event");
const dronesRoute = require("./routes/drone");
const radarRoute = require("./routes/radar");


app.use(cors());
app.use(express.json());


app.use("/api/auth", authRoute);
app.use("/api/events", eventRoute);
app.use("/api/drones", dronesRoute);
app.use("/api/radars", radarRoute);


app.listen(port, () => {
  console.log(`Server started on port ${port}`);
});