const Sequelize = require("sequelize");
const db = require("../config/database");

const Drone = db.define(
  "drone",
  {
    uuid: {
      type: Sequelize.UUID,
      defaultValue: Sequelize.UUIDV4,
    },
    type: {
      type: Sequelize.STRING,
    },
  },
  {
    tableName: "tbl_drone",
  }
);

module.exports = Drone;
