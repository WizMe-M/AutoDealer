﻿// Global using directives

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json.Serialization;
global using AutoDealer.API.Abstractions;
global using AutoDealer.API.BodyTypes;
global using AutoDealer.API.Configs;
global using AutoDealer.API.Extensions;
global using AutoDealer.API.Services;
global using AutoDealer.DAL;
global using AutoDealer.DAL.Database;
global using AutoDealer.DAL.Database.Entity;
global using AutoDealer.DAL.Repositories;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApplicationModels;
global using Microsoft.AspNetCore.Mvc.Routing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Npgsql;