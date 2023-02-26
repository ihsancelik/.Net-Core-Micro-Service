<template>
  <ValidationObserver ref="observer">
    <v-layout class="d-md-flex justify-center ml-5">
      <v-card class="mx-auto pa-10" outlined elevation="5">
        <h2 class="text-md-center">Miracle</h2>
        <hr class="mb-5" />

        <v-form>
          <ValidationProvider #default="{ errors }" :name="translator('email')" rules="required|email">
            <v-text-field
              v-model="email"
              :error-messages="errors"
              :label="translator('email')"
              type="email"
              prepend-inner-icon="mdi-mail"
              required
              outlined
            >
            </v-text-field>
          </ValidationProvider>

          <v-btn class="v-btn v-btn--block primary v-size--default mb-1" style="color: white;" @click="send()">
            {{ translator("sendResetPasswordEmail") }}
          </v-btn>

          <router-link style="text-decoration-line: none; float: right;" tag="a" to="/account/login"
            ><a>{{ translator("cancel") }}</a></router-link
          >
        </v-form>
      </v-card>
    </v-layout>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>
<script>
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "../../helpers/components/MessageBox";
import AccountService from "../../services/AccountService";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("email", { ...rules.email, message: "Email can be valid" });

export default {
  data() {
    return {
      email: "",

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
    async send() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new AccountService().forgotPasswordRequest(this.email);
        if (response.success) await this.$router.push("/");
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
  },
};
</script>

<style scoped></style>
