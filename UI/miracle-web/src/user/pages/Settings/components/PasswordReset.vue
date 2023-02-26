<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center pt-0">{{ translator("passwordReset") }}</h2>
      <hr class="mb-3" />

      <ValidationProvider #default="{ errors }" name="Password" rules="required|min:8" vid="password">
        <v-text-field
          ref="password"
          v-model="user.password"
          :error-messages="errors"
          label="Password"
          required
          outlined
          prepend-inner-icon="mdi-lock"
          :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
          :type="showPassword ? 'text' : 'password'"
          @click:append="showPassword = !showPassword"
        ></v-text-field>
      </ValidationProvider>
      <ValidationProvider #default="{ errors }" name="Re-Password" rules="required|confirmed:password|min:8">
        <v-text-field
          ref="password"
          v-model="user.repassword"
          :error-messages="errors"
          label="Re-Password"
          required
          outlined
          prepend-inner-icon="mdi-lock"
          :append-icon="showRePassword ? 'mdi-eye' : 'mdi-eye-off'"
          :type="showRePassword ? 'text' : 'password'"
          @click:append="showRePassword = !showRePassword"
        ></v-text-field>
      </ValidationProvider>

      <v-btn class="v-btn v-btn--block primary v-size--default mb-1" style="color: white;" @click="resetPassword"
        >Reset Password</v-btn
      >
    </v-card>
  </ValidationObserver>
</template>
<script>
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import * as rules from "vee-validate/dist/rules";
import AccountService from "@/services/AccountService";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("min", { ...rules.min, message: "{_field_} may not be less than {length} characters" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  props: ["userIdProp"],
  data() {
    return {
      user: {},
      showPassword: false,
      showRePassword: false,

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
    async resetPassword() {
      let response = await new AccountService().passwordReset(this.user);
      if (response.success) this.close(true);
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {},
};
</script>
