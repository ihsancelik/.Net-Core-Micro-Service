<template>
  <v-row justify="center">
    <v-col cols="12" sm="6" md="4" lg="3">
      <ValidationObserver ref="observer">
        <v-card class="mx-auto pa-5" outlined elevation="5">
          <h2 class="text-md-center">Miracle</h2>
          <hr />
          <br />
          <v-card-text>
            <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:64">
              <v-text-field
                ref="name"
                v-model="user.name"
                :error-messages="errors"
                :label="translator('name')"
                required
                prepend-inner-icon="mdi-card"
              ></v-text-field>
            </ValidationProvider>
            <ValidationProvider #default="{ errors }" :name="translator('surname')" rules="required|max:64">
              <v-text-field
                ref="surname"
                v-model="user.surname"
                :error-messages="errors"
                :label="translator('surname')"
                required
                prepend-inner-icon="mdi-card"
              ></v-text-field>
            </ValidationProvider>
            <ValidationProvider
              #default="{ errors }"
              :name="translator('phone')"
              rules="required|min:10|max:11|numeric"
            >
              <v-text-field
                ref="number"
                v-model="user.phoneNumber"
                :error-messages="errors"
                :label="translator('phone')"
                counter="11"
                required
                prepend-inner-icon="mdi-phone"
              ></v-text-field>
            </ValidationProvider>
            <ValidationProvider #default="{ errors }" :name="translator('email')" rules="required|email">
              <v-text-field
                ref="email"
                v-model="user.email"
                :error-messages="errors"
                :label="translator('email')"
                required
                prepend-inner-icon="mdi-email"
              ></v-text-field>
            </ValidationProvider>
            <ValidationProvider #default="{ errors }" :name="translator('username')" rules="required|max:32">
              <v-text-field
                ref="username"
                v-model="user.username"
                :error-messages="errors"
                :label="translator('username')"
                required
                prepend-inner-icon="mdi-account"
              ></v-text-field>
            </ValidationProvider>
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
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                :type="showPassword ? 'text' : 'password'"
                @click:append="showPassword = !showPassword"
                prepend-inner-icon="mdi-key"
              ></v-text-field>
            </ValidationProvider>
            <ValidationProvider
              #default="{ errors }"
              :name="translator('rePassword')"
              rules="required|confirmed:password|min:8"
            >
              <v-text-field
                v-model="user.rePassword"
                :error-messages="errors"
                :label="translator('rePassword')"
                required
                :append-icon="showRePassword ? 'mdi-eye' : 'mdi-eye-off'"
                :type="showRePassword ? 'text' : 'password'"
                @click:append="showRePassword = !showRePassword"
                prepend-inner-icon="mdi-key"
              ></v-text-field>
            </ValidationProvider>
          </v-card-text>
          <v-divider class="mt-12"></v-divider>
          <v-card-actions>
            <v-btn @click="clear" text>{{ translator("clear") }}</v-btn>
            <v-spacer></v-spacer>
            <v-btn color="primary" text @click.prevent="register()">{{ translator("register") }}</v-btn>
            <v-spacer></v-spacer>
            <router-link style="text-decoration-line: none; float: right;" tag="a" to="/account/login">
              <v-btn text>{{ translator("cancel") }}</v-btn>
            </router-link>
          </v-card-actions>
        </v-card>

        <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
          <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
        </v-dialog>
      </ValidationObserver>
    </v-col>
  </v-row>
</template>

<script>
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "@/helpers/components/MessageBox";
import AccountService from "@/services/AccountService";

setInteractionMode("eager");
extend("confirmed", { ...rules.confirmed, message: "{_field_} can be match" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("min", { ...rules.min, message: "{_field_} may not be less than {length} characters" });
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });
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
    clear() {
      this.user = {};
    },
    async register() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new AccountService().registerUser(this.user);
        if (response.success) this.$router.push("/");
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
