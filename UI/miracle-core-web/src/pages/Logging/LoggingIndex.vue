<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      style="width: 100%;"
      :headers="headers"
      :items="logs"
      :search="search"
      class="elevation-5"
      :items-per-page="parseInt('5')"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("logs") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-text-field
            style="max-width: 20%;"
            v-model="search"
            append-icon="mdi-magnify"
            :label="translator('search')"
            single-line
            hide-details
          ></v-text-field>
          <v-spacer></v-spacer>
          <v-btn color="primary" dark class="mb-2" @click="filterDialogOpen">
            {{ translator("filter") }}
          </v-btn>
        </v-toolbar>
      </template>
    </v-data-table>

    <v-dialog v-model="dialog" max-width="600" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </v-layout>
</template>

<script>
import FilterOptions from "./components/FilterOptions";
import LoggingService from "@/services/LoggingService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("date"), value: "date" },
        { text: "IP", value: "ip" },
        { text: this.translator("username"), value: "username" },
        { text: this.translator("requestMethod"), value: "requestMethod" },
        { text: this.translator("path"), value: "path" },
        { text: this.translator("statusCode"), value: "statusCode" },
        { text: this.translator("elapsedTime"), value: "elapsedTime" },
        { text: this.translator("userAgent"), value: "userAgent" },
      ],
      logs: [],
      loggingModel: {},
      search: "",
      dateData: {},
      date: new Date().toISOString().substr(0, 10),

      dialog: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  created() {
    this.initialize();
  },
  methods: {
    initialize() {
      let dateArr = this.date.split("-");
      this.dateData.year = dateArr[0];
      this.dateData.month = dateArr[1];
      this.dateData.day = dateArr[2];
      this.loggingModel = {
        day: this.dateData.day,
        month: this.dateData.month,
        year: this.dateData.year,
        username: "",
      };
      this.getLogs();
    },
    async getLogs() {
      let response = await new LoggingService().logInitialize(this.loggingModel);
      if (response === ReturnConstraints.REFRESH) this.initialize();
      this.logs = [];
      for (let i = response.apiLogs.length - 1; i > 0; i--) {
        let arr = response.apiLogs[i].toString().split(",");
        this.logs.push({
          date: arr[0],
          elapsedTime: arr[6],
          ip: arr[1],
          path: arr[4],
          requestMethod: arr[3],
          statusCode: arr[5],
          userAgent: arr[7],
          username: arr[2],
        });
      }
    },
    filterDialogOpen() {
      this.selectedComponent = FilterOptions;
      this.componentProps = null;

      this.dialog = true;
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.getLogs();
      }

      this.dialog = false;
    },
    setReturnValues(values) {
      if (this.selectedComponent === FilterOptions) {
        this.loggingModel = { ...values };
      }
    },
  },
};
</script>

<style scoped></style>
