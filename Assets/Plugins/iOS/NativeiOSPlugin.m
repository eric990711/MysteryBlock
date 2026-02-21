//
//  NativeiOSPlugin.m
//  test
//
//  Created by donghyun song on 2014. 3. 18..
//  Copyright (c) 2014년 donghyun song. All rights reserved.
//

/*
 #import "NativeiOSPlugin.h"
 
 @implementation NativeiOSPlugin
 
 @end
 */
#include "TSServerConnector.h"


//sample code
void iOSPluginHelloWorld(const char * strMessage)
{
    NSLog(@"iOS log 1");
    UnitySendMessage("iOSManager" , "SetLog" , strMessage);
    NSLog(@"iOS log 2");
}


void iOSGoPlatform()
{
    [TSSC goPlatform];
}

void iOSCheckPlatform()
{
    bool bflag = [TSSC checkPlatform];
    
    UnitySendMessage("GameApplication", "TSANReciveInfo", bflag?"checkPlatform:1":"checkPlatform:0");
    
    
}


void iOSGetPlatformURL()
{
    const char* str= [[TSSC getURL] UTF8String];
	NSLog(@"iOSGetPlatformURL GetURL %@" , [TSSC getURL] );
    UnitySendMessage("iOSManager", "AppReciveMsg", str);
    UnitySendMessage("iOSManager", "SetLog" , str );
}

void iOSLogout()
{
    [TSSC logout];
    //UnitySendMessage("iOSManager" , "SetLog" , )
}

void iOSStartGame()
{
    [TSSC startGame];
}

void iOSLogin(const char *str)
{
    
    NSString *email;
    NSString *passwd;
    
    [TSSC loginUser:@"test30@email.com" passwd:@"111111"
         completion:^{
         }success:^(NSDictionary *response){
             UnitySendMessage("iOSManager", "SetLog" , [[response description]UTF8String]);
         } fail:^(NSDictionary *response ){
             UnitySendMessage("iOSManager", "SetLog" , [[response description]UTF8String]);
         }
     
     ];
    
}


void iOSSetUserData(const char *str)
{
    // NSString *data = str;
    NSString *sBuff = [[NSString alloc] initWithUTF8String:str];
    
    NSLog(@"iossetuserdata%@" , sBuff );
    
    
    [TSSC setUserInfo:sBuff];
    
}

void iOSGetUserData()
{
    [TSSC sendUnityMsg:@"AppReciveMsg" strdata:[[TSSC getInfo]description]];
}


void iOSgoUrl(const char *str)
{
    NSString *sBuff = [[NSString alloc] initWithUTF8String:str];
    
    [TSSC goUrl:sBuff];
}

void iOSgoApp(const char *packageName , const char *iUrl )
{
    NSString *sBuff1 = [[NSString alloc] initWithUTF8String:packageName];
    NSString *sBuff2 = [[NSString alloc] initWithUTF8String:iUrl];
    
    [TSSC goApp:sBuff1 storeUrl: sBuff2] ;
    
}


