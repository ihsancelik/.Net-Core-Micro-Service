<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-card>
      <v-tabs fixed-tabs v-model="tab" background-color="#37474f" dark>
        <v-tab v-for="item in items" :key="item.tab">
          {{ item.tab }}
        </v-tab>
      </v-tabs>

      <v-tabs-items v-model="tab" style="height: 700px; margin-top: 50px;">
        <v-tab-item v-for="item in items" :key="item.tab">
          <component :is="item.content" />
        </v-tab-item>
      </v-tabs-items>
    </v-card>
  </v-layout>
</template>

<script>
import About from "../Settings/About/About";
import ContactInfo from "../Settings/Contact/ContactInfo";
import SmtpSettings from "../Settings/SmtpSetting/SmtpSettings";
export default {
  data() {
    return {
      tab: null,
      items: [
        { tab: "About Settings", content: About },
        { tab: "Contact Info", content: ContactInfo },
        { tab: "Smtp Settings", content: SmtpSettings },
      ],
    };
  },
};
</script>

<style scoped></style>
