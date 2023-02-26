<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("newSmtpSettings") }}</h2>
      <hr class="mb-6" />

      <v-form>
        <ValidationProvider #default="{ errors }" name="Host" rules="required|max:32">
          <v-text-field
            class="field"
            v-model="smtpSetting.host"
            :error-messages="errors"
            label="SMTP Host"
            required
            outlined
            counter="32"
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" name="Port" rules="required|numeric|max:6">
          <v-text-field
            class="field"
            v-model="smtpSetting.port"
            :error-messages="errors"
            label="SMTP Port"
            required
            outlined
            counter="6"
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('email')" rules="required|email">
          <v-text-field
            class="field"
            v-model="smtpSetting.email"
            :error-messages="errors"
            :label="'SMTP ' + translator('email')"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider
          #default="{ errors }"
          :name="translator('password')"
          vid="password"
          rules="required|confirmed:password|min:8"
        >
          <v-text-field
            class="field"
            v-model="smtpSetting.password"
            @click:append="showPassword = !showPassword"
            :type="showPassword ? 'text' : 'password'"
            :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
            :label="'SMTP ' + translator('password')"
            :error-messages="errors"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>

        <v-switch v-model="smtpSetting.enableSSL" label="Enable SSL" required outlined></v-switch>
        <hr class="mb-2" />
        <v-btn @click="smtpSettingCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("create") }}
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
import SmtpSettingService from "@/services/SmtpSettingService";

setInteractionMode("eager");
extend("email", { ...rules.email, message: "Email can be valid" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      smtpSetting: {},
      showPassword: false,
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async smtpSettingCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new SmtpSettingService().create(this.smtpSetting);
        if (response.success) this.close("closed", true);
        else {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
        }
      }
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
  created() {
    this.smtpSetting.enableSSL = false;
  },
};
</script>

<style scoped>
.field {
  margin-bottom: -4%;
}
</style>
