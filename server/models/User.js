const Sequelize = require("sequelize");
const db = require("../config/database");
const { ROLE } = require("../config/roles");
const User = db.define(
  "user",
  {
    id: {
      type: Sequelize.INTEGER,
      primaryKey: true,
      unique: true,
    },
    name: {
      type: Sequelize.STRING,
    },
    uuid: {
      type: Sequelize.UUID,
      defaultValue: Sequelize.UUIDV4,
    },
    username: {
      type: Sequelize.STRING,
      allowNull: false,
    },
    password: {
      type: Sequelize.STRING,
      allowNull: false,
    },
    role: {
      type: Sequelize.STRING,
      defaultValue: ROLE.DEFAULT,
    },
  },
  {
    tableName: "tbl_user",
  }
);

module.exports = User;
