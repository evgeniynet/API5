// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Collections.Generic;
using System.Net;
using System;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Data;

namespace SherpaDeskApi.ServiceInterface
{
    public class PaymentsService : Service
    {
        [Secure("tech")]
        public object Get(Payment request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckPaymentEnable(instanceConfig);

            return Models.Payment.GetPayment(hdUser.OrganizationId, hdUser.DepartmentId, request.id);
        }

        public static void CheckPaymentEnable(Instance_Config instanceConfig)
        {
            if (!instanceConfig.EnablePayments) throw new HttpError("Payments is not enabled for this instance.");
        }
    }
}
