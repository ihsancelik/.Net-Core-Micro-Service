<template>
  <v-card>
    <v-card-title class="headline">{{ translator("filter") }}</v-card-title>
    <v-card-text>
      <v-select
        v-model="username"
        :items="users"
        item-value="username"
        item-text="username"
        :placeholder="translator('selectUsername')"
        prepend-icon="mdi-account"
      >
        <template #append-outer>
          <v-btn text @click="username = ''">
            <v-icon large>mdi-close</v-icon>
          </v-btn>
        </template>
      </v-select>
      <v-spacer></v-spacer>
      <v-dialog ref="dialog" v-model="modal" :return-value.sync="date" persistent width="290px">
        <template #activator="{ on }">
          <v-text-field v-model="date" prepend-icon="mdi-calendar" readonly v-on="on"></v-text-field>
        </template>
        <v-date-picker v-model="date" scrollable>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="modal = false"> {{ translator("close") }} </v-btn>
          <v-btn text color="primary" @click="$refs.dialog.save(date)"> {{ translator("save") }} </v-btn>
        </v-date-picker>
      </v-dialog>
      <v-spacer></v-spacer>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-spacer></v-spacer>

      <v-btn color="green darken-1" text @click="getLogs(date)">
        {{ translator("ok") }}
      </v-btn>

      <v-btn color="green darken-1" text @click="close(false)">
        {{ translator("close") }}
      </v-btn>
    </v-card-actions>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </v-card>
</template>

<script>
import UserService from "@/services/UserService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  created() {
    this.initialize();
  },
  data() {
    return {
      logs: [],
      users: [],
      username: "",
      modal: false,
      dateData: {},
      date: new Date().toISOString().substr(0, 10),
      signalModel: {
        changes: false,
        returnValues: [],
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  methods: {
    async initialize() {
      let response = await new UserService().getListAll();
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.users = response.list;
    },
    getLogs(date) {
      let dateArr = date.split("-");
      this.dateData.year = dateArr[0];
      this.dateData.month = dateArr[1];
      this.dateData.day = dateArr[2];
      let model = {
        day: this.dateData.day,
        month: this.dateData.month,
        year: this.dateData.year,
        username: this.username,
      };

      this.signalModel.returnValues = model;
      this.close(true);
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

<style scoped></style>
