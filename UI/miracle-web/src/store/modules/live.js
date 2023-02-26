/* eslint-disable */
const state = {
  listMessage: [],
  roomList: [],
  message: "",
  room: "",
};

const getters = {
  room(state) {
    return state.room;
  },
  message(state) {
    return state.message;
  },
  roomList(state) {
    return state.roomList;
  },
  listMessage(state) {
    return state.listMessage;
  },
};

const mutations = {
  setInfo(state, room, message) {
    state.room = room;
    state.message = message;
  },

  setRoomName(state, value) {
    state.room = value;
  },

  setMessage(value, state) {
    state.message = value;
  },
  setRoomList(state, value) {
    state.roomList = value;
  },

  setListMessage(value, state) {
    state.listMessage = value;
  },

  div(message) {
    let p = document.createElement("p");
    let x = document.getElementsByClassName("pp");
    p.id = "new-message";
    p.textContent = message;
  },
};

const actions = {
  userJoinRoom({ commit, state }, roomName) {
    commit("setRoomName", roomName);
    state.room = roomName;
    return state.room;
  },

  userSendMessage({ commit, state }, roomName, message) {
    commit("setRoomName", roomName);

    state.listMessage.push(message);
    state.roomList.push(roomName);

   

    return state.listMessage;
  },
  div(state) {
    commit("setRoomName", roomName);
    commit("div", state.listMessage);
    return state.listMessage
  },

  //Admine, userın mesajları gitsin
  listenMessageForAdmin({ commit, state }) {
    commit("setInfo", state.room, state.message);

    state.listMessage.push(state.message);
    state.roomList.push(state.room);
    return state.listMessage;
  },
};

export default {
  state,
  getters,
  mutations,
  actions,
};
