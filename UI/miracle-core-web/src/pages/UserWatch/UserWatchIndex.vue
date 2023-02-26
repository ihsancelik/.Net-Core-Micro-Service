<template>
  <v-container>
    <v-layout class="child-flex text-center justify-center ml-5">
      <v-card class="cardStyle" elevation="10">
        <v-sheet class="v-sheet--offset mx-auto mb-5 pb-2" color="rgb(0, 210, 0)" elevation="8">
          <span>{{ onlineUserCount }} / {{ totalOnlineUserCount }} </span>
          <div class="title font-weight-bold text-center">
            {{ translator("onlineUsers") }}
          </div>
        </v-sheet>
        <v-card-text class="pt-0">
          <v-data-table :headers="headers" :items="users" group-by="companyName" :loading="loading" />
        </v-card-text>
      </v-card>
    </v-layout>
  </v-container>
</template>

<script>
import UserWatchService from "@/services/UserWatchService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      loading: true,
      headers: [
        { text: "CompanyName", value: "companyName" },
        { text: "Username", value: "username" },
        { text: "Name", value: "name" },
        { text: "Surname", value: "surname" },
        { text: "Email", value: "email" },
      ],
      users: [],
      totalOnlineUserCount: 0,
      onlineUserCount: 0,
    };
  },
  created() {
    this.initialize();
    this.loading = false;
  },
  methods: {
    async initialize() {
      let onlineUserResponse = await new UserWatchService().getOnlineUsers();
      let countResponse = await new UserWatchService().getCount();
      if ((onlineUserResponse || countResponse) === ReturnConstraints.REFRESH) await this.initialize();

      this.users = onlineUserResponse.list;
      this.onlineUserCount = this.users.length;
      this.totalOnlineUserCount = countResponse.data;
    },
  },
};
</script>

<style scoped>
.cardStyle {
  width: 100%;
  margin-bottom: 10px;
  font-size: 50px;
}
</style>
