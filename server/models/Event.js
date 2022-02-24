const Sequelize = require("sequelize");
const db = require("../config/database");
const Event = db.define(
  "event",
  {
    uuid: {
      type: Sequelize.UUID,
      defaultValue: Sequelize.UUIDV4,
    },
    type: {
      type: Sequelize.STRING,
    },
    location: {
      type: Sequelize.STRING,
      require: true,
    },
    status: {
      type: Sequelize.STRING,
      defaultValue: "open",
    },
    end_time: {
      type: Sequelize.DATE,
    },
  },
  {
    tableName: "tbl_event",
  }
);

module.exports = Event;
