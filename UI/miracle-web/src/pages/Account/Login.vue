<template>
  <ValidationObserver ref="observer" @keypress.enter="login">
    <v-layout class="d-md-flex justify-center ml-5">
      <v-card class="mx-auto pa-10" outlined elevation="5">
        <h2 class="text-md-center">
          <router-link to="/" style="text-decoration: none; color: black;">Miracle</router-link>
        </h2>
        <hr class="mb-5" />

        <v-form>
          <ValidationProvider #default="{ errors }" :name="translator('username')" rules="required">
            <v-text-field
              v-model="user.username"
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
              v-model="user.password"
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
          <router-link style="text-decoration-line: none;" tag="a" to="forgot-password-req"
            ><a>{{ translator("forgotPassword") }}</a></router-link
          >

          <hr class="mb-5" />

          <v-btn
            class="v-btn v-btn--block primary v-size--default mb-1"
            :disabled="!isValid"
            style="color: white;"
            @click="login()"
          >
            {{ translator("login") }}
          </v-btn>

          <router-link to="register">
            <v-btn class="v-btn v-btn--block v-size--default mb-1" secondary outlined>
              {{ translator("register") }}
            </v-btn>
          </router-link>
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
import MessageBox from "@/helpers/components/MessageBox";
import AccountService from "@/services/AccountService";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      user: {
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
        let response = await new AccountService().login(this.user);
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
      return !(this.user.username === "" || this.user.password === "");
    },
  },
};
</script>
