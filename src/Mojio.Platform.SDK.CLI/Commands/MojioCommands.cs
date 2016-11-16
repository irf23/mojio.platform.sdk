﻿using Mojio.Platform.SDK.Contracts;
using Mojio.Platform.SDK.Contracts.Entities;
using Mojio.Platform.SDK.Entities.DI;
using System;
using System.Threading;
using System.Threading.Tasks;
using Mojio.Platform.SDK.Contracts.Push;

namespace Mojio.Platform.SDK.CLI.Commands
{
    [CommandDescriptor(Name = "rename-mojio", Description = "Rename a mojio", Usage = "rename-mojio /id:<mojio id> /name:<mojio name>")]
    public class RenameMojioCommand : BaseCommand
    {
        [Argument(ArgumentType.Required, ShortName = "i")]
        public string Id { get; set; }

        [Argument(ArgumentType.Required, ShortName = "n")]
        public string Name { get; set; }

        public override async Task Execute()
        {
            await Authorize();

            var g = Guid.Empty;
            if (Guid.TryParse(Id, out g))
            {
                var result = await SimpleClient.RenameMojio(g, Name);
                Log.Debug(result);
                UpdateAuthorization();
            }
            else
            {
                Log.Info("Invalid Id specified.");
            }
        }
    }

    [CommandDescriptor(Name = "get-mojios", Description = "List out all mojios", Usage = "get-mojios")]
    public class GetMojiosCommand : BaseCommand
    {
        [Argument(ArgumentType.AtMostOnce, ShortName = "t")]
        public int Top { get; set; }

        [Argument(ArgumentType.AtMostOnce, ShortName = "s")]
        public int Skip { get; set; }

        [Argument(ArgumentType.AtMostOnce, ShortName = "f")]
        public string Filter { get; set; }

        [Argument(ArgumentType.AtMostOnce, ShortName = "se")]
        public string Select { get; set; }

        [Argument(ArgumentType.AtMostOnce, ShortName = "order")]
        public string OrderBy { get; set; }

        public override async Task Execute()
        {
            await Authorize();

            var result = await SimpleClient.Mojios(Skip, Top, Filter, Select, OrderBy);
            Log.Debug(result);
            UpdateAuthorization();
        }
    }

    [CommandDescriptor(Name = "claim-mojio", Description = "Claim a mojio", Usage = "claim-mojio /imei:99991234567890987")]
    public class MojioCommands : BaseCommand
    {
        [Argument(ArgumentType.Required, ShortName = "i")]
        public string Imei { get; set; }

        public override async Task Execute()
        {
            await Authorize();

            var result = await SimpleClient.ClaimMojio(Imei);
            Log.Debug(result);
            UpdateAuthorization();
        }
    }

    [CommandDescriptor(Name = "delete-mojio", Description = "Delete a mojio", Usage = "delete-mojio /id:<mojio-id>")]
    public class DeleteMojioCommand : BaseCommand
    {
        [Argument(ArgumentType.Required, ShortName = "i")]
        public string Id { get; set; }

        public override async Task Execute()
        {
            await Authorize();

            var g = Guid.Empty;
            if (Guid.TryParse(Id, out g))
            {
                var result = await SimpleClient.DeleteMojio(g);
                Log.Debug(result);
                UpdateAuthorization();
            }
            else
            {
                Log.Info("Invalid Id specified.");
            }
        }
    }

#if DOTNETCORE

    [CommandDescriptor(Name = "watch-mojio", Description = "Begin watching a mojio or all mojios",
         Usage = "watch-mojio /id:MojioId")]
    public class WatchMojoCommand : BaseCommand
    {
        [Argument(ArgumentType.AtMostOnce, ShortName = "id")]
        public string Id { get; set; } = null;

        public override async Task Execute()
        {
            await Authorize();

            var watch = DIContainer.Current.Resolve<IWatchMojios>();
            if (watch != null)
            {
                var observer = DIContainer.Current.Resolve<IObserver<IMojio>>();
                var observeable = await watch.WatchMojios(SimpleClient, Id, DIContainer.Current.Resolve<CancellationToken>(), vehicle =>
                {
                    Log.Debug(vehicle);
                });
                observeable.Subscribe(observer);
            }
            else
            {
                Log.Debug("Could not watch the vehicle. App level issue.");
            }

            //                var result = await SimpleClient.VehicleTrips(g, Skip, Top, Filter, Select, OrderBy);
            //                Log.Debug(result);
            UpdateAuthorization();
        }
    }

#endif

    [CommandDescriptor(Name = "list-watched-mojios", Description = "List currently watched mojios",
         Usage = "list-watched-mojios")]
    public class ListWatchedMojiosCommand : BaseCommand
    {
        [Argument(ArgumentType.AtMostOnce, ShortName = "id")]
        public string Id { get; set; } = null;

        public override async Task Execute()
        {
            await Authorize();

            var result = await SimpleClient.GetObservers(ObserverEntity.Mojios);
            Log.Debug(result);

            UpdateAuthorization();
        }
    }
}