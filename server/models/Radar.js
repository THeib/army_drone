const Sequelize = require("sequelize");
const db = require("../config/database");

const Radar = db.define(
  "radar",
  {
    uuid: {
      type: Sequelize.UUID,
      defaultValue: Sequelize.UUIDV4,
    },
    location: {
      type: Sequelize.STRING,
      require: true,
    },
  },
  {
    tableName: "tbl_radar",
  }
);

module.exports = Radar;
