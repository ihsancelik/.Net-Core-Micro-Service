<template>
  <v-container>
    <v-card elevation="10">
      <v-sheet class="v-sheet--offset mx-auto mb-5 pb-3" style="margin-left: 3px;" color="blue" elevation="8">
        <div class="cardStyle">
          {{ translator("startDate") }} : {{ startDate }} <br />
          {{ translator("currentDate") }} : {{ currentDate }} <br />
          {{ translator("totalRequestCount") }} : {{ serverInfo.totalRequestCount }}
        </div>
      </v-sheet>

      <v-data-table :headers="headers" :items="errorList" :loading="loading" class="elevation-5">
        <template #top>
          <v-toolbar flat color="white">
            <v-toolbar-title>{{ translator("serverInfo") }}</v-toolbar-title>
            <v-divider class="mx-4" inset vertical></v-divider>
            <v-select
              v-model="selectedDependency"
              :items="dependencies"
              item-text="libName"
              item-value="libName"
              chips
              class="select"
            ></v-select>
          </v-toolbar>
        </template>
        <template #item.dependencyExceptions="{ item }">
          <v-list-item>
            {{ item.errorMsg }}
          </v-list-item>
        </template>
      </v-data-table>
    </v-card>
  </v-container>
</template>

<script>
import ServerInfoService from "@/services/ServerInfoService";
import DependencyService from "@/services/DependencyService";

export default {
  created() {
    this.initialize();
    this.loading = false;
  },
  data() {
    return {
      headers: [{ text: "Dependency Manager Exception", value: "dependencyExceptions" }],
      serverInfo: {},
      startDate: new Date(),
      currentDate: new Date(),
      errorList: [],
      dependencies: [],
      selectedDependency: "",
    };
  },

  methods: {
    async initialize() {
      let serverResponse = await new ServerInfoService().getInfo();
      this.serverInfo = serverResponse.data;
      this.startDate = new Date(this.serverInfo.startDate).toISOString().substr(0, 19).replace("T", " ");
      this.currentDate = new Date(this.serverInfo.currentDate).toISOString().substr(0, 19).replace("T", " ");

      let dependencyResponse = await new DependencyService().getListAll();
      this.dependencies = dependencyResponse.list;
    },

    async getDependencyExceptions(libName) {
      let serverResponse = await new ServerInfoService().getDependencyExceptions(libName);
      if (serverResponse.errorList !== null) {
        serverResponse.errorList.forEach((error) => {
          this.errorList.push({ errorMsg: error });
        });
      }
    },
  },
  watch: {
    selectedDependency(newVal) {
      this.getDependencyExceptions(newVal);
    },
  },
};
</script>

<style scoped>
.cardStyle {
  width: 100%;
  margin: 10px;
  font-size: 25px;
  text-align: justify;
}

.select {
  max-width: 20%;
  margin-top: 2%;
}
</style>
