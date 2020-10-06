import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthGuardService, AuthInterceptor } from './services/auth.service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ChatComponent } from './chat/chat.component';
import { ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ChatMessageReceiverComponent } from './chat-message-receiver/chat-message-receiver.component';
import { ChatMessageSenderComponent } from './chat-message-sender/chat-message-sender.component';
import { MomentModule } from 'ngx-moment';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    ChatComponent,
    ChatMessageReceiverComponent,
    ChatMessageSenderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter(request) {
          return localStorage.getItem("token");
        },
      },
    }),
    ReactiveFormsModule,
    FontAwesomeModule,
    MomentModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }, AuthGuardService],
  // providers: [AuthGuardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
