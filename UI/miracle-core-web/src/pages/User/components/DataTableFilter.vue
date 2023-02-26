<template>
  <v-card class="pa-10 pt-5" elevation="5">
    <a @click="closeDialog(false)">
      <v-icon style="float: right;">mdi-close-circle</v-icon>
    </a>
    <v-card-title class="headline">{{ translator("filter") }}</v-card-title>
    <hr class="mb-3" />

    <form class="center">
      <v-spacer></v-spacer>
      <v-select
        :placeholder="translator('selectFilter')"
        v-model="filterParameters.propertyName"
        item-value="value"
        item-text="name"
        :items="[
          { name: translator('name'), value: 'Name' },
          { name: translator('username'), value: 'Username' },
          { name: translator('email'), value: 'Email' },
        ]"
        class="text-field"
      ></v-select>

      <v-spacer></v-spacer>
      <v-select
        :placeholder="translator('selectType')"
        v-model="filterParameters.filterType"
        item-value="value"
        item-text="name"
        :items="[
          { name: translator('contains'), value: 'Contains' },
          { name: translator('equals'), value: 'Equals' },
        ]"
        class="text-field"
      ></v-select>

      <v-spacer></v-spacer>
      <v-text-field
        append-icon="mdi-magnify"
        v-model="filterParameters.searchValue"
        :label="translator('search')"
        single-line
        hide-details
        class="text-field"
      ></v-text-field>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="green darken-1" text @click="reset()">{{ translator("reset") }}</v-btn>
        <v-btn color="green darken-1" text @click="closeDialog(true)">{{ translator("ok") }}</v-btn>
      </v-card-actions>
    </form>
  </v-card>
</template>

<script>
export default {
  props: ["propertyNameProp", "filterTypeProp", "searchValueProp"],
  data() {
    return {
      name: null,
      username: null,
      email: null,

      filterParameters: {
        page: 1,
        pageSize: -1,
        propertyName: "",
        filterType: "",
        searchValue: "",
      },
      signalModel: {
        changes: false,
        returnValues: null,
      },
    };
  },
  methods: {
    reset() {
      this.filterParameters.propertyName = null;
      this.filterParameters.filterType = null;
      this.filterParameters.searchValue = null;
    },
    closeDialog(changes) {
      this.signalModel.changes = changes;
      if (changes) this.signalModel.returnValues = this.filterParameters;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {
    this.filterParameters.propertyName = this.propertyNameProp;
    this.filterParameters.filterType = this.filterTypeProp;
    this.filterParameters.searchValue = this.searchValueProp;
  },
};
</script>

<style scoped>
.text-field {
  max-width: 400px;
}
</style>
