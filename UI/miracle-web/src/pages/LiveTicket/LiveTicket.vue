<template>
  <ValidationObserver ref="observer">
    <v-card class="chat-window" outlined elevation="5">
      <!-- Header -->
      <v-sheet style="width: 100%; height: 10%; background-color: #4e8cff;">
        <v-row>
          <v-col cols="10" md="8">
            <h2 class="text-md-center" style="font-family: ui-monospace; color: white;">{{ translator("chat") }}</h2>
          </v-col>
          <v-col style="text-align: -webkit-center !important; margin-left: -8%;">
            <a @click="close(false)">
              <v-icon class="mt-1 ml-5" style="73px !important" color="white">mdi-close-circle</v-icon>
            </a>
          </v-col>
        </v-row>
      </v-sheet>

      <!-- Giriş Ekranı -->
      <div v-if="click == false">
        <h4 style="margin-bottom: 80%; margin-left: 20%;"><v-icon>mdi-account</v-icon> Bağlanıyor..</h4>
      </div>

      <div
        v-if="click == true && existingUser === false"
        style="margin-top: inherit; margin-bottom: auto; height: auto;"
      >
        <div v-if="userInfo" class="text-center margin-top: inherit; margin-bottom: auto; height: auto;">
          <p style="text-align: center; margin: 15% 3%; border-radius: 20px;">
            {{ translator("welcome") }}
          </p>
          <ValidationProvider #default="{ errors }" :name="translator('username')" rules="required">
            <input class="girdi" type="text" v-model="roomModel.customerName" :placeholder="translator('fullName')" />
          </ValidationProvider>
          <ValidationProvider #default="{ errors }" :name="translator('company')" rules="required">
            <input
              class="girdi"
              type="text"
              v-model="roomModel.company"
              @keyup.enter.enter="chatStart()"
              :placeholder="translator('company')"
            />
          </ValidationProvider>
          <div>
            <v-btn @click.prevent="chatStart()" large outlined color="black" width="80%" class="mt-5">
              Live Support Start
            </v-btn>
          </div>
        </div>
      </div>

      <!-- Mesajları Listele -->
      <v-row
        v-if="contentList == true || existingUser === true"
        class="text-center yazisma"
        style="margin-top: 4%;"
        :style="{ maxWidth: '100%', maxHeight: '100%', overflowY: 'scroll', padding: '5px 5px' }"
      >
        <br />
        <v-col style="max-height: inherit; padding-top: 4%;">
          <p class="helloText">Merhaba {{ roomModel.customerName }}, bugün size nasıl yardımcı olabiliriz?</p>
          <div v-for="(content, list) in roomModel.messages" :key="list">
            <div v-if="!content.isAdmin">
              <ul class="customer mt-2">
                <li>
                  <b>{{ roomModel.customerName }} </b> : {{ content.message }}
                </li>
              </ul>
            </div>
            <div v-else>
              <ul class="admin mt-2">
                <li>
                  <b>{{ content.adminName }} </b> : {{ content.message }}
                </li>
              </ul>
            </div>
          </div>
        </v-col>
      </v-row>

      <!-- Yaz-Gönder -->
      <v-row v-if="!msgInput || existingUser == true" class="yaz-row">
        <v-col cols="12" md="10">
          <input
            type="text"
            v-model="messageModel.message"
            @keyup.enter.enter="send()"
            :placeholder="translator('liveWrite')"
            id="message"
            class="messages"
          />
        </v-col>
        <v-col cols="12" md="2">
          <v-icon style="margin-top: 1px; margin-left: -15px; margin-left: -8%;" @click="send()">mdi-send</v-icon>
        </v-col>
      </v-row>
    </v-card>
  </ValidationObserver>
</template>

<script>
/*eslint-disable*/
import * as signalR from "@aspnet/signalr";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import * as rules from "vee-validate/dist/rules";
import LiveTicketService from "@/services/LiveTicketService";
import NewsCreateVue from "../../admin/pages/News/components/NewsCreate.vue";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  name: "LiveTicket",
  components: { ValidationObserver, ValidationProvider },

  data() {
    return {
      connection: "",

      userInfo: true,
      msgInput: true,
      click: false,
      contentList: false,
      notification: false,

      existingUser: false,

      roomModel: {
        roomName: localStorage.getItem("room"),
        customerName: "",
        company: "",
        isConnected: false,
        messages: [],
      },

      messageModel: {
        isAdmin: false,
        adminName: "",
        message: "",
      },

      signalModel: {
        changes: false,
        returnValues: undefined,
      },

      dialogEnable: false,
      dialogKey: 0,

      chats: [],
    };
  },

  created() {
    // #region Connection
    this.connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl("http://127.0.0.1:5103/chat", {
        accessTokenFactory: () => {
          return localStorage.getItem("webToken");
        },
        transport: signalR.HttpTransportType.WebSockets,
      })
      //.withHubProtocol([0, 60, 120, 3000, 5000, 10000, 15000, 30000])
      .withAutomaticReconnect([0, 60, 120, 3000, 5000, 10000, 15000, 30000])
      .build();
    // #endregion

    // #region Start
    try {
      this.connection.start().then(() => {
        this.connection.invoke("JoinRoomUser", this.roomModel.roomName).then(() => {
          console.info("Connection State -> " + this.connection.state);
          console.info("Group Name : " + this.roomModel.roomName);
        });
      });

      //Yeniden bağlanma girişimlerini başlatmadan önce fırlatılan event’tir.
      this.connection.onreconnecting(() => {
        console.assert(this.connection.state === signalR.HubConnectionState.Reconnecting);
        console.info("Connection Onreconnecting -> " + this.connection.state);
      });

      //Yeniden bağlantı gerçekleştirildiğinde fırlatılır.
      this.connection.onreconnected(() => {
        console.assert(this.connection.state === signalR.HubConnectionState.Connected);
        console.info("Connection Onreconnected -> " + this.connection.state);
        // localStorage.clear();
      });
    } catch (error) {
      console.error(error);
      setTimeout(function () {
        this.connection.start();
      }, 1000);
    }
    this.connection.onclose(() => {
      console.info("Connection OnClose -> " + this.connection.connectionState);
    });

    // #endregion

    this.connection.on("ReceiveAdminInfoToUser", (adminName) => {
      this.messageModel.adminName = adminName;
    });

    this.connection.on("ReceiveClickRoom", (click, roomName) => {
      this.click = true;

      new LiveTicketService().getContents(roomName).then((content) => {
        if (content.list != null) {
          this.roomModel.messages = content.list;
          this.existingUser = true;
        }

        if (localStorage.getItem("chats") != null) {
          new LiveTicketService().getChats(roomName).then((response) => {
            this.roomModel.customerName = response.list[0].customerName;
          });
        }
      });
    });

    this.connection.on("ReceiveMessage", (customerName, adminName, isAdmin, message) => {
      this.roomModel.customerName = customerName;
      this.roomModel.messages.push({
        adminName: adminName,
        isAdmin: isAdmin,
        message: message,
      });

      new LiveTicketService().getContents(this.roomModel.roomName).then((response) => {
        if (response.list == null) return;
        this.roomModel.messages = response.list;
        this.existingUser = true;
      });
    });
  },

  methods: {
    chatStart() {
      let startDate = new Date().toISOString().substr(0, 10);

      if (localStorage.getItem("chats") == null) {
        this.chats.push({
          to: this.roomModel.customerName,
          company: this.roomModel.company,
          startDate: startDate,
        });
        localStorage.setItem("chats", JSON.stringify(this.chats));
      }

      this.msgInput = false;
      this.userInfo = false;
      this.contentList = true;
    },

    send() {
      this.userInfo = false;

      let sendMessageModel = {
        customerName: this.roomModel.customerName,
        adminName: this.messageModel.adminName,
        message: this.messageModel.message,
        company: this.roomModel.company,
        roomName: this.roomModel.roomName,
        isAdmin: false,
      };
      this.roomModel.messages.push(sendMessageModel);

      this.connection.invoke("SendMessageToRoom", sendMessageModel);
      this.connection.invoke("SendNotification", sendMessageModel.roomName, true);

      new LiveTicketService().getContents(sendMessageModel.roomName).then((response) => {
        if (response.list == null) return;

        this.roomModel.messages = response.list;
        this.existingUser = true;
      });
    },

    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },

    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
};
</script>

<style scoped>
.yazisma {
  margin-left: unset;
  margin-right: unset;
  height: -webkit-fill-available;
}
.yaz-row {
  margin-left: unset;
  margin-right: unset;
  margin-top: 6px;
  background-color: wheat;
  height: 56px;
  border-style: groot;
  margin-bottom: -4px;
}
.customer {
  font-family: -webkit-pictograph;
  background-color: rgb(190, 216, 226);
  border-radius: 10px 10px 10px 10px;
  font-size: 16px;
  max-width: 86%;
  /* display: inline; */
  float: right;
}
.admin {
  font-family: -webkit-pictograph;
  background-color: rgb(232, 196, 243);
  border-radius: 10px 10px 10px 10px;
  font-size: 16px;
  max-width: 90%;
  float: left;
}

.messages {
  height: 50px;
  width: -webkit-fill-available;
  float: left;
  border-radius: 20px;
  border-color: transparent !important;
  margin-top: -9px;
  font-family: -webkit-pictograph;
}
.helloText {
  margin-left: 5%;
  text-align: left;
}
.girdi {
  text-align: center;
  float: left;
  margin: 4% 0px;
  width: -webkit-fill-available;
}

ul {
  display: block;
  min-width: 70%;
  max-width: 90%;
  list-style-type: none;
  text-align: start;
}

/* Genel */
.chat-window {
  width: 350px;
  height: calc(100% - 120px);
  max-height: 550px;
  position: fixed;
  right: 3%;
  bottom: 100px;
  box-sizing: border-box;
  box-shadow: 0px 7px 40px 2px rgba(148, 149, 150, 0.1);
  background: white;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  border-radius: 10px;
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  animation: fadeIn;
  animation-duration: 0.3s;
  animation-timing-function: ease-in-out;
}

::placeholder {
  color: rgb(165, 165, 218);
  font-family: Arial, Helvetica, sans-serif;
  font-size: 16px;
}

input:focus,
button:focus {
  outline: none;
}
</style>
