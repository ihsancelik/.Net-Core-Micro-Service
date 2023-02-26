<template>
  <v-card class="text-center">
    <v-card-title class="headline">{{ messageTitle }}</v-card-title>
    <br />
    <v-card-text style="font-size: medium">{{ messageText }}</v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="green darken-1" text @click="close(true)">{{ translator("ok") }}</v-btn>
      <v-btn
        v-if="cancelButtonVisible"
        color="green darken-1"
        text
        @click="close(false)"
      >{{ translator("close") }}</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  props: ["messageTitleProp", "messageTextProp", "cancelButtonVisibleProp"],
  created() {
    this.messageTitle = this.messageTitleProp;
    this.messageText = this.messageTextProp;
    this.cancelButtonVisible = this.cancelButtonVisibleProp;
  },
  data() {
    return {
      messageTitle: "",
      messageText: "",
      cancelButtonVisible: false
    };
  },
  methods: {
   close(changes) {
      const signalModel={
        changes: changes,
        returnValues : null
      };
      this.$emit("closed", signalModel);
    },
  },
};
</script>

<style scoped></style>