package com.example.internship;

import java.util.List;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface StudentApi {
    @GET("api/student/{userId}")
    Call<List<StudentDto>> getStudentDtos(@Path("userId") String userId, @Header("Authorization") String token);

    @FormUrlEncoded
    @POST("Login")
    Call<DataPlusAccessToken> loginUser(
            @Field("username") String username,
            @Field("password") String password,
            @Field("grant_type") String grantType
    );

    @GET("api/course")
    Call<List<Course>> getCourses();

    @POST("api/student")
    Call<String> postStudent(
            @Body StudentDto dtoObject,
            @Header("Authorization") String token
    );

    @DELETE("api/student/{id}")
    Call<String> deleteStudent(@Path("id") int id, @Header("Authorization") String token);

    @GET("api/student/{id}")
    Call<StudentDto> getOneStudent(@Path("id") int id, @Header("Authorization") String token);

    @PUT("api/student")
    Call<String> editStudent(@Body StudentDto dtoObject,@Header("Authorization") String token);
}
