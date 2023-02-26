<template>
  <ValidationObserver ref="observer" @keypress.enter="login">
    <v-layout class="d-md-flex justify-center ml-5">
      <v-card class="mx-auto pa-10" outlined elevation="5">
        <h2 class="text-md-center">Miracle</h2>
        <hr class="mb-5" />

        <v-form>
          <ValidationProvider #default="{ errors }" :name="translator('username')" rules="required">
            <v-text-field
              v-model="userModel.username"
              :error-messages="errors"
              :label="translator('username')"
              required
              outlined
              prepend-inner-icon="mdi-account"
            >
            </v-text-field>
          </ValidationProvider>

          <ValidationProvider #default="{ errors }" :name="translator('password')" rules="required">
            <v-text-field
              ref="password"
              v-model="userModel.password"
              :error-messages="errors"
              :label="translator('password')"
              required
              outlined
              prepend-inner-icon="mdi-lock"
              :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
              :type="showPassword ? 'text' : 'password'"
              @click:append="showPassword = !showPassword"
            ></v-text-field>
          </ValidationProvider>

          <router-link style="text-decoration-line: none;" tag="a" to="forgot-password-req">
            <a> {{ translator("forgotPassword") }} </a>
          </router-link>

          <hr class="mb-5" />
          <v-btn
            class="v-btn v-btn--block primary v-size--default mb-1"
            :disabled="!isValid"
            style="color: white;"
            height="40"
            @click="login()"
          >
            {{ translator("login") }}
          </v-btn>
        </v-form>
      </v-card>
    </v-layout>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "@/helpers/Components/MessageBox";
import AccountService from "@/services/AccountService";

setInteractionMode("eager");

extend("required", {
  ...rules.required,
  message: "{_field_} can not be empty",
});
export default {
  data() {
    return {
      userModel: {
        password: "",
        username: "",
      },
      showPassword: false,

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  methods: {
    async login() {
      if (this.isValid) {
        let response = await new AccountService().login(this.userModel);
        if (response !== undefined) {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
        }
      } else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("error"),
          messageTextProp: "Username or Password cannot be empty",
        };
        this.dialogEnable = true;
        this.dialogKey += 1;
      }
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
  },
  computed: {
    isValid() {
      return !(this.userModel.username === "" || this.userModel.password === "");
    },
  },
};
</script>
