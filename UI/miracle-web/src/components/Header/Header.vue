<template>
  <v-row align="center" class="headerRow ma-0 fill-height text-center" justify="center">
    <v-col cols="12">
      <h1>{{ title.toUpperCase() }}</h1>
    </v-col>
  </v-row>
</template>

<script>
export default {
  props: ["title"],
  name: "Header",
};
</script>

<style scoped>
.headerRow {
  width: 100%;
  height: 30%;
  max-height: 200px;
  background-color: #eeeeff;
}
@media only screen and (max-width: 750px) {
  .headerRow {
    max-height: 100px;
    height: 10%;
  }
}
</style>
