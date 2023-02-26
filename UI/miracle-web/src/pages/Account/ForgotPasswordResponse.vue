<template>
  <ValidationObserver ref="observer">
    <v-layout class="d-md-flex justify-center ml-5">
      <v-card class="mx-auto pa-10" outlined elevation="5">
        <h2 class="text-md-center">Miracle</h2>
        <hr class="mb-5" />

        <v-form>
          <ValidationProvider
            #default="{ errors }"
            :name="translator('password')"
            rules="required|min:8"
            vid="password"
          >
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
          <ValidationProvider
            #default="{ errors }"
            :name="translator('rePassword')"
            rules="required|confirmed:password|min:8"
          >
            <v-text-field
              ref="password"
              v-model="user.repassword"
              :error-messages="errors"
              :label="translator('rePassword')"
              required
              outlined
              prepend-inner-icon="mdi-lock"
              :append-icon="showRePassword ? 'mdi-eye' : 'mdi-eye-off'"
              :type="showRePassword ? 'text' : 'password'"
              @click:append="showRePassword = !showRePassword"
            ></v-text-field>
          </ValidationProvider>

          <v-btn class="v-btn v-btn--block primary v-size--default mb-1" style="color: white;" @click="send()">
            {{ translator("resetPassword") }}
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
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "../../helpers/components/MessageBox";
import AccountService from "../../services/AccountService";

setInteractionMode("eager");
extend("confirmed", { ...rules.confirmed, message: "{_field_} can be match" });
extend("min", { ...rules.min, message: "{_field_} may not be less than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      user: {},
      showPassword: false,
      showRePassword: false,

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
    async send() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new AccountService().forgotPasswordResponse(this.user);
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
  beforeMount() {
    this.user.code = this.$route.params.code;
  },
};
</script>

<style scoped></style>
