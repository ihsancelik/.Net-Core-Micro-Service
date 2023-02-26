<template>
  <v-card elevation="10" style="height: 900px; width: -webkit-fill-available; margin-left: 1%; margin-right: 1%;">
    <v-row style="height: 790px; max-width: 100%; margin: 0px 0px;">
      <v-col cols="12" md="3" style="border-style: inset; height: 900px;">
        <div style="height: 80px;" class="mb-1">
          <h2 style="text-align: -webkit-center;" class="mt-5">{{ translator("onlineUsers") }}</h2>
        </div>

        <div v-if="roomList.length > 0" style="height: 774px;" :style="{ maxHeight: '100px;', overflowY: 'scroll' }">
          <div v-for="(messageModel, list) in roomList" :key="list" class="onlineUser mt-3">
            <a @click="onRoomClicked(messageModel)" id="clickChat">
              <div v-if="messageModel.notification.show == true" class="notification">
                {{ messageModel.notification.count }}
              </div>
              <h3 class="mt-5">{{ messageModel.roomName }}</h3>
            </a>
          </div>
        </div>
      </v-col>

      <v-col cols="12" md="9" style="border-style: inset; height: 900px;">
        <div style="height: 80px;" class="mb-3">
          <h2 style="text-align: -webkit-center;" class="mt-3">{{ translator("messages") }}</h2>
          <a v-if="isIcon == true" @click="iconShow()" style="margin-left: 90%;">
            <v-icon class="mt-1 ml-5" style="73px !important" color="black">mdi-close-circle</v-icon>
          </a>

          <div v-for="(messageModel, list) in roomList" :key="list" class="mt-3">
            <p v-if="messageModel.click == true && isIcon == true" style="font-size: xx-large; margin-top: -2%;">
              to: {{ messageModel.roomName }} - {{ messageModel.customerName }}
            </p>
          </div>
        </div>

        <div
          v-if="showMessage == true && selectedRoom != undefined"
          style="height: 665px; margin-top: 3%;"
          :style="{ maxHeight: '772px;', overflowY: 'scroll' }"
        >
          <v-row v-for="(messageModel, list) in selectedRoom.messageModels" :key="list" class="mt-3">
            <v-col v-if="messageModel.isAdmin" cols="12" class="admin mt-5">
              <div class="mt-5">
                <p class="mt-1">
                  <b>{{ messageModel.adminName }}</b> : {{ messageModel.message }}
                </p>
              </div>
            </v-col>
            <v-col v-else cols="12" class="customer mt-3">
              <div style="margin-top: 1%;">
                <p class="mt-1">
                  <b>{{ selectedRoom.customerName }}</b> : {{ messageModel.message }}
                </p>
              </div>
            </v-col>
          </v-row>
        </div>
        <div v-if="isIcon == true" style="border-style: inset; height: 77px; margin-top: 10px;">
          <input
            type="text"
            v-model="message"
            @keyup.enter.enter="send()"
            :placeholder="translator('liveWrite')"
            class="write"
          />
          <v-icon style="margin-top: 2.5%; margin-left: 2%;" @click="send()">mdi-send</v-icon>
        </div>
      </v-col>
    </v-row>
  </v-card>
</template>

<script>
/*eslint-disable*/
import * as signalR from "@aspnet/signalr";
import LiveTicketService from "@/services/LiveTicketService";

export default {
  name: "LiveTicketManager",

  data() {
    return {
      message: "",
      adminName: "",

      isIcon: false,
      showMessage: false,

      clickedRoom: "",
      chats: [],
      notifications: [],

      roomList: [],
      selectedRoom: {
        isConnected: false,
        customerName: "",
        company: "",
        messageModels: [],
        click: false,
        notification: {
          roomName: "",
          count: 0,
          show: false,
        },
      },

      signalModel: {
        changes: false,
        returnValues: undefined,
      },
      dialogEnable: false,
      dialogKey: 0,
    };
  },

  created() {
    // #region Connection
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5103/chat", {
        accessTokenFactory: () => {
          return localStorage.getItem("webToken");
        },
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect([0, 60, 120, 3000, 5000, 10000, 15000, 30000])
      .configureLogging(signalR.LogLevel.Information)
      .build();
    // #endregion

    // #region Start
    try {
      this.connection
        .start()
        .then(() => {
          this.connection.invoke("JoinRoomAdmin");
          console.info("Connection State -> " + this.connection.state);
        })
        .catch((err) => console.error(err));
    } catch (error) {
      console.error(error);
      setTimeout(function () {
        this.connection.start();
        console.info("Connection State -> " + this.connection.state);
      }, 10000);
    }

    //Yeniden bağlanma girişimlerini başlatmadan önce fırlatılan event’tir.
    this.connection.onreconnecting(() => {
      console.assert(this.connection.state === signalR.HubConnectionState.Reconnecting);
      console.info("Connection Onreconnecting -> " + this.connection.state);
    });

    //Yeniden bağlantı gerçekleştirildiğinde fırlatılır.
    this.connection.onreconnected(() => {
      console.assert(this.connection.state === signalR.HubConnectionState.Connected);
      console.info("Connection Onreconnected -> " + this.connection.state);
    });

    this.connection.onclose(() => {
      console.assert(this.connection.state === signalR.HubConnectionState.Disconnected);
      console.info("Connection OnClose -> " + this.connection.connectionState);
    });
    // #endregion

    this.connection.on("ReceiveAdminInfo", (adminName) => {
      this.adminName = adminName;
    });

    this.connection.on("OnListenRoom", (room) => {
      // User chat iconunu tıkladığı an çalışır
      new LiveTicketService().getListAll().then((response) => {
        let currentRoom = response.list.find((s) => s.roomName == room);
        if (currentRoom == undefined || currentRoom == "") {
          this.roomList.push({
            roomName: room,
            customerName: "",
            company: "",
            isConnected: false,
            messageModels: [],
            click: false,
            notification: {
              roomName: "",
              count: 0,
              show: false,
            },
          });
        }
      });
    });

    this.connection.on("OnListenMessageToAdmin", (customerName, isAdmin, message, customerRoom, company) => {
      let currentRoom = this.roomList.find((s) => s.roomName == customerRoom);

      if (currentRoom.click == true) {
        if (this.clickedRoom == this.selectedRoom.roomName) {
          if (this.selectedRoom.liveChatContents == undefined || this.selectedRoom.liveChatContents.lenght > 0) {
            //Admin not refresh
            currentRoom.customerName = customerName;
            currentRoom.company = company;

            currentRoom.messageModels.push({
              isAdmin: isAdmin,
              message: message,
              adminName: this.adminName,
            });

            this.selectedRoom = currentRoom;
            new LiveTicketService().getChats(currentRoom.roomName).then((chat) => {
              if (chat.list == null) return;

              this.selectedRoom = chat.list[0];
              this.selectedRoom.messageModels = chat.list[0].liveChatContents;

              currentRoom.messageModels = this.selectedRoom.messageModels;
            });

            this.showMessage = true;
            this.isIcon = true;
          } else {
            //Admin refresh
            new LiveTicketService().getChats(currentRoom.roomName).then((chat) => {
              if (chat.list == null) return;

              this.selectedRoom = chat.list[0];
              this.selectedRoom.messageModels = chat.list[0].liveChatContents;
              currentRoom.messageModels = this.selectedRoom.messageModels;
            });

            this.showMessage = true;
            this.isIcon = true;
          }
        }
      } else {
        //Admin refresh
        if (this.selectedRoom.messageModels != undefined || this.selectedRoom.messageModels > 0) {
          if (currentRoom.messageModels != undefined || currentRoom.messageModels > 0) {
            //Refresh olmadan önce
            if (currentRoom.roomName) {
              currentRoom.roomName = customerRoom;
              currentRoom.customerName = customerName;
              currentRoom.company = company;

              currentRoom.messageModels.push({
                isAdmin: isAdmin,
                message: message,
                adminName: this.adminName,
              });

              this.selectedRoom.messageModels = currentRoom.messageModels;
            }
          } else {
            currentRoom.roomName = customerRoom;
            currentRoom.customerName = customerName;
            currentRoom.company = company;
            this.selectedRoom = currentRoom;

            currentRoom.messageModels = [];
            currentRoom.messageModels.push({
              isAdmin: isAdmin,
              message: message,
              adminName: this.adminName,
            });

            new LiveTicketService().getChats(currentRoom.roomName).then((chat) => {
              if (chat.list == null) return;

              this.selectedRoom = chat.list[0];
              this.selectedRoom.messageModels = chat.list[0].liveChatContents;
              currentRoom.messageModels = this.selectedRoom.messageModels;
            });

            this.selectedRoom.messageModels = currentRoom.messageModels;
          }
        }
      }

      // İcon change control
      if (this.clickedRoom != this.selectedRoom.roomName || this.clickedRoom == "") {
        this.showMessage = false;
        this.isIcon = false;
      }
    });

    this.connection.on("NotificationToAdmin", (customerRoom, notification) => {
      let currentRoom = this.roomList.find((s) => s.roomName == customerRoom);

      if (currentRoom.click == false) {
        var notifications = JSON.parse(localStorage.getItem("notification"));
        this.notifications = notifications;

        if (this.notifications != null) {
          let notification = notifications.find((s) => s.roomName == customerRoom);

          if (notification == null) {
            notifications.push({
              show: true,
              count: 1,
              roomName: customerRoom,
            });

            this.notifications.forEach((element, index, array) => {
              if (this.notifications[index].roomName == customerRoom) {
                currentRoom.notification = this.notifications[index];
              }

              if (this.notifications[index].roomName == this.selectedRoom.customerName) {
                this.selectedRoom.notification = this.notifications[index];
              }
            });

            this.roomList.forEach((element, index, array) => {
              if (this.roomList[index].roomName == customerRoom) {
                this.roomList[index].notification = currentRoom.notification;
              }
            });
          }

          if (notification != null) {
            this.notifications.forEach((element, index, array) => {
              if (element.roomName == customerRoom) {
                this.notifications[index].count += 1;
                this.notifications[index].show = true;
                this.notifications[index].roomName = customerRoom;
              }

              if (customerRoom == this.selectedRoom.customerName) {
                this.selectedRoom.notification = this.notifications[index];
              }
            });

            this.roomList.forEach((element, index, array) => {
              if (element.roomName == customerRoom) {
                this.roomList[index].notification = notification;
              }
            });
          }

          localStorage.setItem("notification", JSON.stringify(this.notifications));
        }

        if (this.notifications == null || this.notifications == undefined || this.notifications == []) {
          this.notifications = [];

          this.notifications.push({
            show: true,
            count: 1,
            roomName: customerRoom,
          });
          localStorage.setItem("notification", JSON.stringify(this.notifications));

          let selectedNotification = this.notifications.find((s) => s.roomName == customerRoom);

          this.roomList.forEach((element, index, array) => {
            if (this.roomList[index].notification.roomName == selectedNotification.roomName) {
              this.roomList[index].notification = selectedNotification;
            }
          });

          this.notifications.forEach((element, index, array) => {
            if (this.notifications[index].roomName == this.selectedRoom.roomName) {
              this.selectedRoom.notification = this.notifications[index];
            }
          });
        }
      }
    });

    this.initialize();
  },

  methods: {
    async initialize() {
      // #region roles
      let roles = localStorage.getItem("roles");
      roles = roles !== null ? roles.split(",") : null;
      roles.forEach((role) => {
        if (role === "Admin" || role === "SoftwareDeveloper") this.isAdmin = true;
      });
      // #endregion

      this.roomList = [];
      let response = await new LiveTicketService().getListAll();
      this.roomList = response.list;

      var notificationList = JSON.parse(localStorage.getItem("notification"));
      this.notifications = notificationList;

      this.roomList.forEach((element, index, array) => {
        var emptyNotification = {
          count: 0,
          show: false,
          roomName: element.roomName,
        };
        this.roomList[index].notification = emptyNotification;

        if (notificationList != undefined || notificationList != null) {
          let selectedNotification = notificationList.find((s) => s.roomName == element.roomName);
          new LiveTicketService().getChats(element.roomName).then((chat) => {
            if (chat.list != null) {
              chat.list.forEach((room, ind, array) => {
                if (room.roomName == element.roomName && chat.list[ind].liveChatContents.length == 0) {
                  let index = this.notifications.indexOf(selectedNotification);
                  this.notifications.splice(index, 1);
                }
              });
            }
            localStorage.setItem("notification", JSON.stringify(this.notifications));
          });

          if (selectedNotification != undefined) {
            this.roomList[index].notification = selectedNotification;
          }
          this.roomList[index].click = false;
        }
      });
    },

    onRoomClicked(room) {
      this.isIcon = true;
      this.showMessage = true;

      let currentRoom = this.roomList.find((s) => s.roomName == room.roomName);

      this.selectedRoom = currentRoom;
      this.clickedRoom = currentRoom.roomName;

      //Admin not refresh
      if (currentRoom.messageModels != undefined) {
        this.selectedRoom = currentRoom;
        currentRoom.messageModels;
      } else {
        new LiveTicketService().getChats(currentRoom.roomName).then((chat) => {
          if (chat.list == null) return;

          this.selectedRoom = chat.list[0];
          this.selectedRoom.messageModels = chat.list[0].liveChatContents;
          currentRoom.messageModels = this.selectedRoom.messageModels;
        });
      }

      //Notification Control
      // localstorage silme
      var data = JSON.parse(localStorage.getItem("notification"));

      if (data != undefined || data == []) {
        let findNotification = data.find((s) => s.roomName == room.roomName);
        if (findNotification != null) {
          let selectedRoomNotification = this.notifications.find((s) => s.roomName == findNotification.roomName);
          let index = this.notifications.indexOf(selectedRoomNotification);

          data.splice(index, 1);
          this.notifications = data;

          findNotification.show = false;
          findNotification.count = 0;
          findNotification.roomName = room.roomName;

          currentRoom.notification = findNotification;
          this.selectedRoom.notification = findNotification;

          localStorage.setItem("notification", JSON.stringify(this.notifications));
        }
      }

      // Seçili room bilgisini gösterme
      this.roomList.forEach((room, index, array) => {
        this.roomList[index].click = false;
        if (room.roomName == currentRoom.roomName) room.click = true;
      });
      this.connection.invoke("ClickRoom", room.click, room.roomName);
    },

    send() {
      let currentRoom = this.roomList.find((s) => s.roomName == this.selectedRoom.roomName);

      let sendMessageRoom = {
        roomName: currentRoom.roomName,
        customerName: currentRoom.customerName,
        company: "test",
        message: this.message, //??
        isAdmin: this.isAdmin,
        adminName: this.adminName,
      };

      this.connection.invoke("SendMessageToRoom", sendMessageRoom).then(() => {
        if (this.selectedRoom.messageModels != undefined) {
          this.selectedRoom.messageModels.push({
            isAdmin: sendMessageRoom.isAdmin,
            adminName: sendMessageRoom.adminName,
            message: sendMessageRoom.message,
          });
        } else {
          new LiveTicketService().getChats(currentRoom.roomName).then((chat) => {
            if (chat.list == null) return;

            this.selectedRoom = chat.list[0];
            currentRoom.messageModels = chat.list[0].liveChatContents;
            this.selectedRoom.messageModels = currentRoom.messageModels;
          });
        }
        this.selectedRoom = currentRoom;
      });

      this.entry = false;
    },

    iconShow() {
      this.isIcon = false;
      this.showMessage = false;
      this.selectedRoom.click = false;
      this.clickedRoom = "";
      this.roomList.forEach((element, index, array) => {
        this.roomList[index].click = false;
      });
    },

    // #region closeDialog
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
    // #endregion
  },
};
</script>

<style scoped>
@media (min-width: 1904px) {
  .container {
    max-width: 1884px !important;
  }
}

.row {
  margin-right: 4px;
  margin-left: 9px;
}
.customer {
  background-color: rgb(204, 227, 236);
  border-color: transparent !important;
  max-width: 50%;
  margin-top: 1%;
  border-radius: 10px;
  float: right !important;
}
.admin {
  float: left !important;
  margin-left: 48%;
  background-color: rgb(237, 197, 250);
  border-color: transparent !important;
  max-width: 50%;
  margin-top: 1%;
  border-radius: 10px;
}
.onlineUser {
  border-style: ridge;
  border-radius: 10px;
  margin: 2% 10%;
  height: 50px;
  text-align: -webkit-center;
}
.notification {
  background-color: red;
  width: 30px;
  height: 30px;
  border-radius: 15px;
  float: right;
  margin-top: -12px;
  color: white;
  margin-top: -5px;
  margin-left: -5px;
  position: relative;
  text-align: center;
}
.write {
  height: 60px;
  width: 90%;
  float: left !important;
  border-color: transparent !important;
  margin: 2px 2% 0px 1%;
  font-family: -webkit-pictograph;
}
input.write {
  font-size: 20px;
  color: rgb(47, 9, 119);
}
.v-application a {
  color: rgb(36, 17, 209);
}

a:active {
  color: rgb(228, 56, 13);
}

input:focus,
button:focus {
  outline: none;
}
</style>
