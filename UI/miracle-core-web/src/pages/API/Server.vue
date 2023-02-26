<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="3">
        <v-btn x-large height="100" width="100%" color="blue" dark>Action 1</v-btn>
      </v-col>
      <v-col cols="12" md="3">
        <v-btn x-large height="100" width="100%" color="primary" dark>Action 2</v-btn>
      </v-col>
      <v-col cols="12" md="3">
        <v-btn x-large height="100" width="100%" color="green" dark>Action 3</v-btn>
      </v-col>
      <v-col cols="12" md="3">
        <v-btn x-large height="100" width="100%" color="purple" dark>Action 4</v-btn>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12" md="6">
        <v-btn x-large height="100" width="100%" color="warning" dark @click="openUserDialog">
          Logout Specific Users
        </v-btn>
      </v-col>
      <v-col cols="12" md="6">
        <v-btn x-large height="100" width="100%" color="red" dark @click="openDialog">Logout All Users</v-btn>
      </v-col>
    </v-row>

    <template>
      <v-row justify="center">
        <v-dialog v-model="dialog" max-width="400" persistent>
          <v-card>
            <v-card-title class="headline">{{ dialogTitle }}</v-card-title>
            <v-card-text>
              This operation will affect all logged in users. If you are sure about that, Please Type Your Username
              <v-text-field v-model="dialogUserName" placeholder="Username" :error="error"></v-text-field>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="green darken-1" text @click="logoutAllUsers">OK</v-btn>
              <v-btn color="green darken-1" text @click="dialog = false">Close</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>
    </template>

    <template>
      <v-row justify="center">
        <v-dialog v-model="userDialog" max-width="400" persistent>
          <v-card>
            <v-card-title class="headline">Select User To Logout</v-card-title>
            <v-card-text>
              <v-select
                v-model="selectedUserId"
                :items="users"
                item-text="username"
                item-value="id"
                placeholder="Select a User"
              ></v-select>
            </v-card-text>
            <v-spacer></v-spacer>
            <v-card-text>
              This operation will affect selected logged in user. If you are sure about that, Please Type Your Username
              <v-text-field v-model="dialogUserName" placeholder="Username" :error="error"></v-text-field>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="green darken-1" text @click="logoutUser">OK</v-btn>
              <v-btn color="green darken-1" text @click="userDialog = false">Close</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>
    </template>
  </v-container>
</template>

<script>
import UserService from "@/services/UserService";
import ServerService from "@/services/ServerService";

export default {
  data() {
    return {
      userDialog: false,
      users: [],
      selectedUserId: 0,
      dialog: false,
      dialogTitle: "Are You Sure?",
      dialogUserName: "",
      error: false,
    };
  },
  methods: {
    async getUserList() {
      let response = await new UserService().getListAll();
      this.users = response.list;
    },
    openDialog() {
      this.dialog = true;
    },
    openUserDialog() {
      this.getUserList();
      this.userDialog = true;
    },
    async logoutUser() {
      let response = await new ServerService().logoutUser(this.selectedUserId, this.dialogUserName);
      if (response) {
        this.dialogUserName = "";
        this.selectedUserId = 0;
        this.userDialog = false;
        this.error = false;
      } else {
        this.error = true;
      }
    },
    async logoutAllUsers() {
      let response = await new ServerService().logoutAllUsers(this.dialogUserName);
      if (response) {
        this.dialog = false;
        this.error = false;
      } else {
        this.error = true;
      }
    },
  },
};
</script>

<style scoped></style>
