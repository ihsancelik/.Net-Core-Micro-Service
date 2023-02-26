<template>
  <v-container>
    <v-row class="mb-5">
      <v-col cols="12" md="1" />

      <v-col cols="12" md="6">
        <v-card-title>{{ translator("contactInfo") }}</v-card-title>
        <p>
          <v-icon large style="margin: 40px 20px;">mdi-map-marker-outline</v-icon>
          <span>{{ contactInfo.address }}</span>
        </p>
        <p>
          <v-icon large style="margin: 40px 20px;">mdi-cellphone</v-icon>
          <span> {{ contactInfo.phone }} </span>
        </p>
        <v-icon large style="margin: 40px 20px;">mdi-email</v-icon>
        <span>{{ contactInfo.email }} </span>
      </v-col>

      <v-col cols="12" md="4">
        <v-card-title>{{ translator("contactForm") }}</v-card-title>
        <v-card-text>
          <v-form class="pr-10">
            <v-text-field
              solo
              :placeholder="translator('fullName') + '*'"
              v-model="contactForm.fullName"
            ></v-text-field>
            <v-text-field solo :placeholder="translator('email') + '*'" v-model="contactForm.email"></v-text-field>
            <v-text-field solo :placeholder="translator('phone') + '*'" v-model="contactForm.phone"></v-text-field>
            <v-textarea solo :placeholder="translator('message') + '*'" v-model="contactForm.message"></v-textarea>
            <v-card-actions>
              <v-btn large outlined color="black" width="30%" @click="send">
                {{ translator("send") }}
              </v-btn>
            </v-card-actions>
          </v-form>
        </v-card-text>
      </v-col>

      <v-col cols="12" md="1" />
    </v-row>

    <v-row>
      <iframe
        width="2000"
        height="600"
        :src="mapSRC"
        frameborder="0"
        scrolling="no"
        marginheight="0"
        marginwidth="0"
      ></iframe>
    </v-row>
    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-container>
</template>

<script>
/* eslint-disable */
import ContactInfoService from "../../services/ContactInfoService";
import ContactFormService from "../../services/ContactFormService";
import MessageBox from "@/helpers/components/MessageBox";

export default {
  data() {
    return {
      contactInfo: {},
      contactForm: {},
      mapSRC: null,

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
    };
  },
  methods: {
    async initialize() {
      let response = await new ContactInfoService().getById(1);
      this.contactInfo = response.data;
      this.mapSRC = this.contactInfo.location;
    },
    async send() {
      let response = await new ContactFormService().create(this.contactForm);
      if (response.success) {
        this.contactForm = {};
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("success"),
          messageTextProp: this.translator("successfullySent"),
          cancelButtonVisibleProp: false,
        };
        this.openDialog();
      }
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
  },
  created() {
    this.initialize();
  },
};
</script>

<style scoped></style>
