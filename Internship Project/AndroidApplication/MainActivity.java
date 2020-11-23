package com.example.internship;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.messaging.FirebaseMessaging;

import org.json.JSONObject;

import java.util.List;

import okhttp3.OkHttpClient;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {
    private TextView username;
    private TextView password;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Button loginButton = findViewById(R.id.loginButton);
        loginButton.setOnClickListener(this);
        generateNotification();
    }

    public void generateNotification(){
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel channel = new NotificationChannel("MyNotifications", "MyNotifications", NotificationManager.IMPORTANCE_DEFAULT);
            NotificationManager notificationManager = getSystemService(NotificationManager.class);
            notificationManager.createNotificationChannel(channel);
        }

        FirebaseMessaging.getInstance().subscribeToTopic("internship")
                .addOnCompleteListener(new OnCompleteListener<Void>() {
                    @Override
                    public void onComplete(@NonNull Task<Void> task) {
                        String msg = "Successful";
                        if (!task.isSuccessful()) {
                            msg = "Failed";
                        }
                        Toast.makeText(MainActivity.this, msg, Toast.LENGTH_SHORT).show();
                    }
                });
    }

    @Override
    public void onClick(View v) {
        username = findViewById(R.id.nameOfUser);
        password = findViewById(R.id.passwordOfUser);
        loginCall();
    }

    public void loginCall(){
        OkHttpClient okHttpClient = UnsafeOkHttpClient.getUnsafeOkHttpClient();
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl("https://192.168.1.8:45455/")
                .client(okHttpClient)
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        StudentApi studentApi = retrofit.create(StudentApi.class);
        Call<DataPlusAccessToken> call = studentApi.loginUser(username.getText().toString(),password.getText().toString(),"password");
        call.enqueue(new Callback<DataPlusAccessToken>() {
            @Override
            public void onResponse(Call<DataPlusAccessToken> call, Response<DataPlusAccessToken> response) {
                Intent intent = new Intent(MainActivity.this,StudentListActivity.class);
                intent.putExtra("userId",response.body().UserId);
                intent.putExtra("token",response.body().access_token);
                startActivity(intent);
            }

            @Override
            public void onFailure(Call<DataPlusAccessToken> call, Throwable t) {
                Log.e("Error", t.getMessage());
            }
        });
    }
}