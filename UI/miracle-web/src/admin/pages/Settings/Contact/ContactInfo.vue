<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5" max-width="50%" style="left: 25%;">
      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('email')" rules="required">
          <v-text-field
            id="field"
            v-model="contactInfo.email"
            :label="translator('email')"
            :error-messages="errors"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('phone')" rules="required">
          <v-text-field
            id="field"
            v-model="contactInfo.phone"
            :label="translator('phone')"
            :error-messages="errors"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('address')" rules="required">
          <v-textarea
            id="field"
            v-model="contactInfo.address"
            :label="translator('address')"
            :error-messages="errors"
            required
            outlined
          ></v-textarea>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('location')" rules="required">
          <v-textarea
            id="field"
            v-model="contactInfo.location"
            :label="translator('location')"
            :error-messages="errors"
            required
            outlined
          ></v-textarea>
        </ValidationProvider>

        <hr class="mb-2" />

        <v-btn @click="contactInfoUpdate" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("update") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "@/helpers/components/MessageBox";
import ContactInfoService from "@/services/ContactInfoService";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  data: () => ({
    contactInfoService: new ContactInfoService(),
    contactInfo: {},

    dialogEnable: false,
    dialogKey: 0,
    selectedComponent: "",
    componentProps: null,
  }),
  methods: {
    initialize() {
      this.contactInfoService.getById(1).then((response) => {
        if (response === 999) this.initialize();
        this.contactInfo = response.data;
      });
    },
    contactInfoUpdate(e) {
      e.preventDefault();
      this.$refs.observer.validate().then((result) => {
        if (result) {
          this.contactInfoService.edit(this.contactInfo, this.contactInfo.id).then((response) => {
            if (response.success) {
              this.selectedComponent = MessageBox;
              this.componentProps = {
                messageTitleProp: this.translator("success"),
                messageTextProp: response.message,
              };
              this.dialogEnable = true;
              this.dialogKey += 1;
            } else {
              this.selectedComponent = MessageBox;
              this.componentProps = {
                messageTitleProp: this.translator("failed"),
                messageTextProp: response.message,
              };
              this.dialogEnable = true;
              this.dialogKey += 1;
            }
          });
        }
      });
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

<style></style>
